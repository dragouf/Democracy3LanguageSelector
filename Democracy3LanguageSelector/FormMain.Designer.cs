namespace Democracy3LanguageSelector
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.comboBoxLanguages = new System.Windows.Forms.ComboBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelGameSourcePath = new System.Windows.Forms.Label();
            this.linkLabelGamePath = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.labelProgression = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelResourcesInfo = new System.Windows.Forms.Label();
            this.progressBarDownlodResources = new System.Windows.Forms.ProgressBar();
            this.folderBrowserDialogGameSource = new Democracy3LanguageSelector.FolderBrowserDialogEx();
            this.toolTipGamePath = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxRemoveSC = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxForceDl = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // comboBoxLanguages
            // 
            this.comboBoxLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLanguages.Items.AddRange(new object[] {
            "Loading languages..."});
            this.comboBoxLanguages.Location = new System.Drawing.Point(109, 12);
            this.comboBoxLanguages.Name = "comboBoxLanguages";
            this.comboBoxLanguages.Size = new System.Drawing.Size(317, 21);
            this.comboBoxLanguages.TabIndex = 0;
            this.comboBoxLanguages.SelectedIndexChanged += new System.EventHandler(this.comboBoxLanguages_SelectedIndexChanged);
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(432, 10);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(108, 23);
            this.buttonApply.TabIndex = 1;
            this.buttonApply.Text = "Apply Translation";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Visible = false;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "Language";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 14);
            this.label2.TabIndex = 3;
            this.label2.Text = "Game Folder";
            // 
            // labelGameSourcePath
            // 
            this.labelGameSourcePath.AutoSize = true;
            this.labelGameSourcePath.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGameSourcePath.Location = new System.Drawing.Point(106, 111);
            this.labelGameSourcePath.MaximumSize = new System.Drawing.Size(326, 14);
            this.labelGameSourcePath.Name = "labelGameSourcePath";
            this.labelGameSourcePath.Size = new System.Drawing.Size(162, 14);
            this.labelGameSourcePath.TabIndex = 4;
            this.labelGameSourcePath.Text = "Please select valid game path...";
            this.labelGameSourcePath.UseMnemonic = false;
            // 
            // linkLabelGamePath
            // 
            this.linkLabelGamePath.AutoSize = true;
            this.linkLabelGamePath.Location = new System.Drawing.Point(489, 111);
            this.linkLabelGamePath.Name = "linkLabelGamePath";
            this.linkLabelGamePath.Size = new System.Drawing.Size(51, 13);
            this.linkLabelGamePath.TabIndex = 5;
            this.linkLabelGamePath.TabStop = true;
            this.linkLabelGamePath.Text = "Browse...";
            this.linkLabelGamePath.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelGamePath_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 14);
            this.label3.TabIndex = 6;
            this.label3.Text = "Progression";
            // 
            // labelProgression
            // 
            this.labelProgression.AutoSize = true;
            this.labelProgression.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProgression.Location = new System.Drawing.Point(106, 143);
            this.labelProgression.MaximumSize = new System.Drawing.Size(330, 14);
            this.labelProgression.Name = "labelProgression";
            this.labelProgression.Size = new System.Drawing.Size(93, 14);
            this.labelProgression.TabIndex = 7;
            this.labelProgression.Text = "Select language...";
            this.labelProgression.UseMnemonic = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 14);
            this.label4.TabIndex = 8;
            this.label4.Text = "Resources";
            // 
            // labelResourcesInfo
            // 
            this.labelResourcesInfo.AutoSize = true;
            this.labelResourcesInfo.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelResourcesInfo.Location = new System.Drawing.Point(106, 175);
            this.labelResourcesInfo.MaximumSize = new System.Drawing.Size(330, 14);
            this.labelResourcesInfo.Name = "labelResourcesInfo";
            this.labelResourcesInfo.Size = new System.Drawing.Size(0, 14);
            this.labelResourcesInfo.TabIndex = 9;
            this.labelResourcesInfo.UseMnemonic = false;
            // 
            // progressBarDownlodResources
            // 
            this.progressBarDownlodResources.Location = new System.Drawing.Point(109, 175);
            this.progressBarDownlodResources.Name = "progressBarDownlodResources";
            this.progressBarDownlodResources.Size = new System.Drawing.Size(317, 14);
            this.progressBarDownlodResources.TabIndex = 10;
            this.progressBarDownlodResources.Visible = false;
            // 
            // folderBrowserDialogGameSource
            // 
            this.folderBrowserDialogGameSource.Description = "";
            this.folderBrowserDialogGameSource.DontIncludeNetworkFoldersBelowDomainLevel = false;
            this.folderBrowserDialogGameSource.NewStyle = true;
            this.folderBrowserDialogGameSource.RootFolder = System.Environment.SpecialFolder.Desktop;
            this.folderBrowserDialogGameSource.SelectedPath = "";
            this.folderBrowserDialogGameSource.ShowBothFilesAndFolders = false;
            this.folderBrowserDialogGameSource.ShowEditBox = true;
            this.folderBrowserDialogGameSource.ShowFullPathInEditBox = true;
            this.folderBrowserDialogGameSource.ShowNewFolderButton = false;
            // 
            // checkBoxRemoveSC
            // 
            this.checkBoxRemoveSC.AutoSize = true;
            this.checkBoxRemoveSC.Checked = true;
            this.checkBoxRemoveSC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRemoveSC.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxRemoveSC.Location = new System.Drawing.Point(109, 47);
            this.checkBoxRemoveSC.Name = "checkBoxRemoveSC";
            this.checkBoxRemoveSC.Size = new System.Drawing.Size(169, 17);
            this.checkBoxRemoveSC.TabIndex = 11;
            this.checkBoxRemoveSC.Text = "Remove before string injection";
            this.checkBoxRemoveSC.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 14);
            this.label5.TabIndex = 12;
            this.label5.Text = "Specials chars";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 14);
            this.label6.TabIndex = 13;
            this.label6.Text = "Download";
            // 
            // checkBoxForceDl
            // 
            this.checkBoxForceDl.AutoSize = true;
            this.checkBoxForceDl.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxForceDl.Location = new System.Drawing.Point(109, 79);
            this.checkBoxForceDl.Name = "checkBoxForceDl";
            this.checkBoxForceDl.Size = new System.Drawing.Size(234, 17);
            this.checkBoxForceDl.TabIndex = 14;
            this.checkBoxForceDl.Text = "Force download translation file if unchanged";
            this.checkBoxForceDl.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 212);
            this.Controls.Add(this.checkBoxForceDl);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkBoxRemoveSC);
            this.Controls.Add(this.progressBarDownlodResources);
            this.Controls.Add(this.labelResourcesInfo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelProgression);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.linkLabelGamePath);
            this.Controls.Add(this.labelGameSourcePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.comboBoxLanguages);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(562, 251);
            this.MinimumSize = new System.Drawing.Size(562, 251);
            this.Name = "FormMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Democracy 3 Language Selector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxLanguages;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelGameSourcePath;
        private System.Windows.Forms.LinkLabel linkLabelGamePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelProgression;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelResourcesInfo;
        private System.Windows.Forms.ProgressBar progressBarDownlodResources;
        private FolderBrowserDialogEx folderBrowserDialogGameSource;
        private System.Windows.Forms.ToolTip toolTipGamePath;
        private System.Windows.Forms.CheckBox checkBoxRemoveSC;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBoxForceDl;
    }
}

