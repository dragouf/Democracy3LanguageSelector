using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Democracy3LanguageSelector
{
    public partial class FormMain : Form
    {
        public Project ProjectInfo { get; set; }
        public TranslationDetails CurrentTranslationInfo { get; set; }
        public Dictionary<string, bool> DownloadingResources { get; set; }

        private DateTime _LastBackupDate;
        public DateTime LastBackupDate
        {
            set
            {
                _LastBackupDate = value;
                var dateText = "Directly download from transifex. If not will use a backup made on {0}".FormatWith(_LastBackupDate.ToShortDateString());
                this.checkBoxLiveUpdate.Invoke((MethodInvoker)delegate
                {
                    this.checkBoxLiveUpdate.Text = dateText; 
                });
            }
            get
            {
                return _LastBackupDate;
            }
        }

        public FormMain()
        {
            InitializeComponent();

            this.DownloadingResources = new Dictionary<string, bool>();
            this.comboBoxLanguages.SelectedIndex = 0;
            this.comboBoxLanguages.Enabled = false;
            this.toolTipGamePath.ToolTipIcon = ToolTipIcon.None;
            this.toolTipGamePath.IsBalloon = false;
            this.toolTipGamePath.ShowAlways = true;

            // Create cache folder
            this.CreateCacheFolderIfNOtExist();

            // Chargement
            LoadGamePath();
            LoadAppSettings();
            LoadLanguagesList();
            LoadBackupDate();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveAppSettings();
        }

        private async void buttonApply_Click(object sender, EventArgs e)
        {
            if (comboBoxLanguages.SelectedItem is Language)
            {
                if (!Directory.Exists(this.labelGameSourcePath.Text))
                {
                    MessageBox.Show("Please select a valid game path...", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Test if restore original files needed
                IsFirstLaunch();

                // if force download redownload
                if (this.checkBoxForceDl.Checked)
                {
                    var previousText = this.labelProgression.Text;
                    this.labelProgression.Text = "Loading transifex translations files...";
                    this.buttonApply.Visible = false;
                    await this.DownloadTranslationFile(this.CurrentTranslationInfo.LanguageCode, this.CurrentTranslationInfo.Total_segments);
                    this.labelProgression.Text = previousText;
                    this.buttonApply.Visible = true;
                    this.progressBarDownlodResources.Visible = false;
                }

                try
                {
                    // list all file from lang cache folder
                    var langCode = ((Language)comboBoxLanguages.SelectedItem).Code;

                    var files = Directory.EnumerateFiles(@"cache\{0}\".FormatWith(langCode), "*.ini");

                    var gameInjector = new DemocracyStringHandling(null, null, this.labelGameSourcePath.Text, this.checkBoxRemoveSC.Checked);

                    // For each file extract and inject
                    foreach (var file in files)
                    {
                        gameInjector.TransifexFilePath = file;
                        gameInjector.InjectTransifexFile();
                    }

                    // Copy additional files if french
                    if (langCode == "fr")
                    {
                        File.Copy(@"Resources\fr\partynames.txt", Path.Combine(this.labelGameSourcePath.Text, "partynames.txt"), overwrite: true);
                        File.Copy(@"Resources\fr\quotes.csv", Path.Combine(this.labelGameSourcePath.Text, "quotes.csv"), overwrite: true);
                    }

                    var dialogResult = MessageBox.Show("Game is now translated in {0}\r\nLaunch game ?".FormatWith(((Language)comboBoxLanguages.SelectedItem).Name), "Translation successful...", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        var exePath = this.labelGameSourcePath.Text.Replace("\\data", "") + "\\Democracy3.exe";
                        ProcessStartInfo psi = new ProcessStartInfo();
                        psi.FileName = exePath;
                        psi.WorkingDirectory = System.IO.Path.GetDirectoryName(psi.FileName);
                        System.Diagnostics.Process.Start(psi);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        
        private async void comboBoxLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonApply.Visible = false;

            if (comboBoxLanguages.SelectedItem is Language)
            {
                this.labelProgression.Text = "Loading transifex translations files...";
                var langCode = ((Language)comboBoxLanguages.SelectedItem).Code;
                int previousProgression = TransifexConnector.GetLastProgressionFromDataFile(langCode);
                this.CurrentTranslationInfo =  await TransifexConnector.TranslationDetails(langCode);

                // Download and Cache files if not currently downloading
                if (!this.DownloadingResources[langCode])
                {
                    // Check if new translations occured (if force download, download will happen when user click apply...)
                    if (previousProgression != this.CurrentTranslationInfo.Translated_segments && !this.checkBoxForceDl.Checked)
                    {
                        await this.DownloadTranslationFile(langCode, this.CurrentTranslationInfo.Total_segments);                       
                    }
                }

                this.buttonApply.Visible = true;
                this.progressBarDownlodResources.Visible = false;

                this.labelProgression.Text = this.CurrentTranslationInfo.PercentProgression.ToString() + " %";
                this.labelResourcesInfo.Text = ProjectInfo.Resources.Count + " files and " + this.CurrentTranslationInfo.Translated_segments.ToString() + "/" + this.CurrentTranslationInfo.Total_segments.ToString() + " translated sentences";
            }
        }

        #region Transifex API
        private async void LoadLanguagesList()
        {
            this.ProjectInfo = await TransifexConnector.ProjectDetails();

            this.ProjectInfo.Teams.ForEach(t => { if(!this.DownloadingResources.ContainsKey(t)) { this.DownloadingResources.Add(t, false); } });

            this.CreateLangueCacheFolderIfNOtExist(this.ProjectInfo.Teams);
            this.comboBoxLanguages.DataSource = new BindingSource(this.ProjectInfo.Languages, null);
            this.comboBoxLanguages.DisplayMember = "Name";
            this.comboBoxLanguages.ValueMember = "Code";
            this.comboBoxLanguages.Text = string.Empty;
            this.comboBoxLanguages.Enabled = true;

            // Previous selected language
            var previousLanguageCode = Properties.Settings.Default.SelectedLanguage;
            var previousSelection = comboBoxLanguages.Items.Cast<Language>().ToList().FirstOrDefault(i => i.Code == previousLanguageCode);
            if (previousSelection != null)
                comboBoxLanguages.SelectedItem = previousSelection;
        }
        private async Task DownloadTranslationFile(string langCode, int totalSegments)
        {
            this.DownloadingResources[langCode] = true;

            // Calculate file size for progress
            this.progressBarDownlodResources.Visible = true;
            this.progressBarDownlodResources.Value = 0;
            this.progressBarDownlodResources.Maximum = TransifexConnector.GetTotalResouresSize(langCode, this.ProjectInfo.Resources, totalSegments);
            this.progressBarDownlodResources.Step = 1024;

            // Progress reporter
            var progress = new Progress<int>();
            progress.ProgressChanged += (s, percent) =>
            {
                progressBarDownlodResources.PerformStep();
            };

            // Download file
            if (this.checkBoxLiveUpdate.Checked)
            {
                // from transifex website
                try
                {
                    await TransifexConnector.DownloadTranslationResources(langCode, this.ProjectInfo.Resources, progress);
                }
                catch
                {
                    var dialogResult = MessageBox.Show(
                        "Transifex website reject download request.\r\nLast available translation can't be download\r\nDo you want to download backup ?", 
                        "Revert changes success", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Error);
                    if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                    {
                    }
                }
            }
            else
            {
                // backuo available in dropbox
                await DropboxConnector.DownloadTranslationResources(langCode, this.ProjectInfo.Resources, progress);
            }

            this.DownloadingResources[langCode] = false;
        }
        #endregion

        #region Dropbox Data
        private async void LoadBackupDate()
        {
            this.LastBackupDate = await DropboxConnector.GetBackupDate();
        }
        #endregion

        #region Labels Link
        private void linkLabelGamePath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.folderBrowserDialogGameSource.SelectedPath = this.labelGameSourcePath.Text;

            var result = this.folderBrowserDialogGameSource.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                var path = this.folderBrowserDialogGameSource.SelectedPath;
                if(!path.Contains("\\data"))
                    path += "\\data"; 
                this.labelGameSourcePath.Text = path;
                this.toolTipGamePath.SetToolTip(this.labelGameSourcePath, this.labelGameSourcePath.Text);
            }
        }

        private void linkLabelRestore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.RestoreOriginalFiles();

            var dialogResult = MessageBox.Show("Originals game files are now restore\r\nLaunch game ?", "Revert changes success", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                var exePath = this.labelGameSourcePath.Text.Replace("\\data", "") + "\\Democracy3.exe";
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = exePath;
                psi.WorkingDirectory = System.IO.Path.GetDirectoryName(psi.FileName);
                System.Diagnostics.Process.Start(psi);
            } 
        }

        private void linkLabelOnline_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (comboBoxLanguages.SelectedItem is Language)
            {
                var langCode = ((Language)comboBoxLanguages.SelectedItem).Code;
                System.Diagnostics.Process.Start("https://www.transifex.com/projects/p/democracy-3/language/{0}/".FormatWith(langCode));
            }
        }
        #endregion

        #region App Settings
        private void LoadAppSettings()
        {
            Properties.Settings.Default.Upgrade();

            this.checkBoxRemoveSC.Checked = Properties.Settings.Default.RemoveSpecialChars;
            this.checkBoxForceDl.Checked = Properties.Settings.Default.ForceDownload;
            this.labelGameSourcePath.Text = string.IsNullOrWhiteSpace(Properties.Settings.Default.GamePath) ? this.labelGameSourcePath.Text : Properties.Settings.Default.GamePath;
        }

        private void SaveAppSettings()
        {
            if (comboBoxLanguages.SelectedItem is Language)
            {
                var langCode = ((Language)comboBoxLanguages.SelectedItem).Code;
                Properties.Settings.Default.SelectedLanguage = langCode;
            }

            Properties.Settings.Default.RemoveSpecialChars = this.checkBoxRemoveSC.Checked;
            Properties.Settings.Default.ForceDownload = this.checkBoxForceDl.Checked;
            Properties.Settings.Default.GamePath = this.labelGameSourcePath.Text; 

            Properties.Settings.Default.Save();
        }
        #endregion

        #region Tools
        private void LoadGamePath()
        {
            try
            {
                string steamInstallPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 245470", "InstallLocation", null);
                string gogInstallPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\GOG.com\GOGDEMOCRACY3", "PATH", null);
                if (!string.IsNullOrEmpty(steamInstallPath) || !string.IsNullOrEmpty(gogInstallPath))
                {
                    if (!string.IsNullOrEmpty(steamInstallPath))
                    {
                        this.labelGameSourcePath.Text = steamInstallPath + "\\data";
                        this.toolTipGamePath.SetToolTip(this.labelGameSourcePath, this.labelGameSourcePath.Text);
                    }
                    else
                    {
                        this.labelGameSourcePath.Text = gogInstallPath + "data";
                        this.toolTipGamePath.SetToolTip(this.labelGameSourcePath, this.labelGameSourcePath.Text);
                    }
                }
                else
                {
                    this.labelGameSourcePath.Text = @"C:\Program Files (x86)\Steam\SteamApps\common\Democracy 3\data";
                    this.toolTipGamePath.SetToolTip(this.labelGameSourcePath, this.labelGameSourcePath.Text);
                }
            }
            catch
            {
                this.labelGameSourcePath.Text = AppDomain.CurrentDomain.BaseDirectory;
                this.toolTipGamePath.SetToolTip(this.labelGameSourcePath, this.labelGameSourcePath.Text);
            }
        }
        private void RestoreOriginalFiles()
        {
            string sourceDir = "Resources\\Originaux\\";
            string targetDir = this.labelGameSourcePath.Text;
            try
            {
                CopyAllFiles(sourceDir, targetDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void IsFirstLaunch()
        {
            if (Properties.Settings.Default.IsFirstLaunch)
            {
                var dialogResult = MessageBox.Show("Since it's first time you are using this version it is recommand to restore original files\r\nDo you want to proceed ?", "First launch actions", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    this.RestoreOriginalFiles();
                }
            }

            Properties.Settings.Default.IsFirstLaunch = false;
        }
        private void CreateCacheFolderIfNOtExist()
        {
            if (!Directory.Exists("cache"))
            {
                Directory.CreateDirectory("cache");
            }
        }
        private void CreateLangueCacheFolderIfNOtExist(List<string> languagesCode)
        {
            foreach (var code in languagesCode)
            {
                if (!Directory.Exists("cache\\" + code))
                    Directory.CreateDirectory("cache\\" + code);
            }
        }
        private void CopyAllFiles(string sourceDir, string targetDir)
        {
            foreach (var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), overwrite:true);

            foreach (var directory in Directory.GetDirectories(sourceDir))
                CopyAllFiles(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
        }
        #endregion                 
    }
}
