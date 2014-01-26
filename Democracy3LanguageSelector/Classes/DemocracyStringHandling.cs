using IniParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Democracy3LanguageSelector
{
    public enum FileType
    {
        MainSentences,
        Mods,
        None
    }
    class DemocracyStringHandling
    {
        public string TransifexFilePath { get; set; }
        public string OutputExtractFolderPath { get; set; }
        public string GameFolderPath { get; set; }
        private bool RemoveSpecialsChars { get; set; }

        public DemocracyStringHandling(string outputFolder, string transifexFile, string gameFolder, bool removeSpecialChars)
        {
            this.TransifexFilePath = transifexFile;
            this.GameFolderPath = gameFolder;
            this.OutputExtractFolderPath = outputFolder;
            this.RemoveSpecialsChars = removeSpecialChars;
        }

        public DemocracyStringHandling(bool removeSpecialChars)
        {
            this.RemoveSpecialsChars = removeSpecialChars;
        }

        #region Extraction
        public void ExtractAllSentences()
        {
            //this.textBoxOutputFolder.Text == OutputExtractFolderPath
            // this.textBoxSource.Text == GameFolderPath
            if (!Directory.Exists(this.OutputExtractFolderPath))
            {
                throw new InvalidOperationException("You must choose an extraction path");
            }

            string outputFilePath = this.OutputExtractFolderPath + "\\Democracy3MainExtractedText.ini";
            if (File.Exists(outputFilePath))
                File.Delete(outputFilePath);

            var iniData = new IniParser.Model.IniData();

            // RACINE
            string path = this.GameFolderPath + "\\";
            var files = Directory.GetFiles(path);
            foreach (var filePath in files)
            {
                var fileName = filePath.Replace(path, "");
                if (fileName == "strings.ini")
                {
                    ParseIni(filePath, fileName, iniData);
                }
                else if (fileName == "tutorial.csv")
                {
                    ParseCsv(filePath, fileName, iniData, 2, new List<int> { 11, 3 });
                }
            }

            // SIMULATION
            path = this.GameFolderPath + "\\simulation\\";
            files = Directory.GetFiles(path);
            foreach (var filePath in files)
            {
                var fileName = filePath.Replace(path, "");
                if (fileName == "achievements.csv")
                {
                    ParseCsv(filePath, fileName, iniData, 1, new List<int> { 4, 2 });
                }
                else if (fileName == "policies.csv")
                {
                    ParseCsv(filePath, fileName, iniData, 1, new List<int> { 4, 2 });
                }
                else if (fileName == "pressuregroups.csv")
                {
                    ParseCsv(filePath, fileName, iniData, 1, new List<int> { 8, 3, 9, 10 });
                }
                else if (fileName == "simulation.csv")
                {
                    ParseCsv(filePath, fileName, iniData, 1, new List<int> { 3, 2 });
                }
                else if (fileName == "situations.csv")
                {
                    ParseCsv(filePath, fileName, iniData, 1, new List<int> { 3, 2, 6, 7 });
                }
                else if (fileName == "votertypes.csv")
                {
                    ParseCsv(filePath, fileName, iniData, 1, new List<int> { 8, 2 });
                }
                else if (fileName == "situations.csv")
                {
                    ParseCsv(filePath, fileName, iniData, 1, new List<int> { 4, 5, 6, 7, 8, 9, 10 });
                }
            }

            // Attacks
            path = this.GameFolderPath + "\\simulation\\attacks\\";
            if (Directory.Exists(path))
            {
                files = Directory.GetFiles(path);
                foreach (var filePath in files)
                {
                    var fileName = filePath.Replace(path, "");
                    ParseIni(filePath, fileName, iniData, new List<string> { "SuccessText", "GUIName" });
                }
            }

            // Dilemmas
            path = this.GameFolderPath + "\\simulation\\dilemmas\\";
            if (Directory.Exists(path))
            {
                files = Directory.GetFiles(path);
                foreach (var filePath in files)
                {
                    var fileName = filePath.Replace(path, "");
                    ParseIni(filePath, fileName, iniData, new List<string> { "description", "guiname", "name" });
                }
            }

            // Events
            path = this.GameFolderPath + "\\simulation\\events\\";
            if (Directory.Exists(path))
            {
                files = Directory.GetFiles(path);
                foreach (var filePath in files)
                {
                    var fileName = filePath.Replace(path, "");
                    ParseIni(filePath, fileName, iniData, new List<string> { "description", "guiname" });
                }
            }

            // Missions
            path = this.GameFolderPath + "\\missions\\";
            if (Directory.Exists(path))
            {
                files = Directory.GetDirectories(path);
                foreach (var filePath in files)
                {
                    var fileName = filePath.Replace(path, "") + ".txt";
                    ParseIni(filePath + "\\" + fileName, fileName, iniData, new List<string> { "description", "guiname" });
                }
            }

            var parser = new IniParser.FileIniDataParser();
            parser.WriteFile(outputFilePath, iniData, Encoding.UTF8);

            //MessageBox.Show("You can now send the file to transifex website\nFile path : " + outputFilePath, "Extraction finished...", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Injection
        public void InjectTransifexFile()
        {
            // parcours les fichier puis essaye de retrouver la partie correspondante dans le fichier de traduction
            if (!File.Exists(this.TransifexFilePath))
            {
                throw new InvalidOperationException("No transifex file");
            }

            var type = DemocracyStringHandling.DetectFileType(this.TransifexFilePath);

            if (type == FileType.MainSentences)
                InjectMain();
            else if (type == FileType.Mods)
            {
                //InjectTitles();
            }
            else
            {
                //throw new InvalidOperationException("File not currently supported");
            }
        }
        private void InjectMain()
        {
            var transifexFileParser = new IniParser.FileIniDataParser();
            var transifexInidata = transifexFileParser.ReadFile(this.TransifexFilePath, Encoding.UTF8);

            // RACINE
            string path = this.GameFolderPath + "\\";
            var files = Directory.GetFiles(path);
            foreach (var filePath in files)
            {
                var fileName = filePath.Replace(path, "");
                // try to find file section in transifex file
                if (transifexInidata.Sections.Any(s => s.SectionName == fileName))
                {
                    var fileSection = transifexInidata.Sections.First(s => s.SectionName == fileName);

                    if (fileName == "strings.ini")
                    {
                        InjectIni(filePath, fileName, fileSection);
                    }
                    else if (fileName == "tutorial.csv")
                    {
                        InjectCsv(filePath, fileName, fileSection);
                    }

                }
            }

            // SIMULATION
            path = this.GameFolderPath + "\\simulation\\";
            files = Directory.GetFiles(path);
            foreach (var filePath in files)
            {
                var fileName = filePath.Replace(path, "");
                // try to find file section in transifex file
                if (transifexInidata.Sections.Any(s => s.SectionName == fileName))
                {
                    var fileSection = transifexInidata.Sections.First(s => s.SectionName == fileName);

                    if (fileName == "achievements.csv")
                    {
                        InjectCsv(filePath, fileName, fileSection);
                    }
                    else if (fileName == "policies.csv")
                    {
                        InjectCsv(filePath, fileName, fileSection);
                    }
                    else if (fileName == "pressuregroups.csv")
                    {
                        InjectCsv(filePath, fileName, fileSection);
                    }
                    else if (fileName == "simulation.csv")
                    {
                        InjectCsv(filePath, fileName, fileSection);
                    }
                    else if (fileName == "situations.csv")
                    {
                        InjectCsv(filePath, fileName, fileSection);
                    }
                    else if (fileName == "votertypes.csv")
                    {
                        InjectCsv(filePath, fileName, fileSection);
                    }
                }
            }

            // Attacks
            path = this.GameFolderPath + "\\simulation\\attacks\\";
            if (Directory.Exists(path))
            {
                files = Directory.GetFiles(path);
                foreach (var filePath in files)
                {
                    var fileName = filePath.Replace(path, "");
                    // try to find file section in transifex file
                    if (transifexInidata.Sections.Any(s => s.SectionName == fileName))
                    {
                        var fileSection = transifexInidata.Sections.First(s => s.SectionName == fileName);
                        InjectIni(filePath, fileName, fileSection);
                    }
                }
            }

            // Dilemmas
            path = this.GameFolderPath + "\\simulation\\dilemmas\\";
            if (Directory.Exists(path))
            {
                files = Directory.GetFiles(path);
                foreach (var filePath in files)
                {
                    var fileName = filePath.Replace(path, "");
                    // try to find file section in transifex file
                    if (transifexInidata.Sections.Any(s => s.SectionName == fileName))
                    {
                        var fileSection = transifexInidata.Sections.First(s => s.SectionName == fileName);
                        InjectIni(filePath, fileName, fileSection);
                    }
                }
            }

            // Events
            path = this.GameFolderPath + "\\simulation\\events\\";
            if (Directory.Exists(path))
            {
                files = Directory.GetFiles(path);
                foreach (var filePath in files)
                {
                    var fileName = filePath.Replace(path, "");
                    // try to find file section in transifex file
                    if (transifexInidata.Sections.Any(s => s.SectionName == fileName))
                    {
                        var fileSection = transifexInidata.Sections.First(s => s.SectionName == fileName);
                        InjectIni(filePath, fileName, fileSection);
                    }
                }
            }

            // Missions
            path = this.GameFolderPath + "\\missions\\";
            if (Directory.Exists(path))
            {
                files = Directory.GetDirectories(path);
                foreach (var filePath in files)
                {
                    var fileName = filePath.Replace(path, "") + ".txt";
                    // try to find file section in transifex file
                    if (transifexInidata.Sections.Any(s => s.SectionName == fileName))
                    {
                        var fileSection = transifexInidata.Sections.First(s => s.SectionName == fileName);
                        InjectIni(filePath + "\\" + fileName, fileName, fileSection);
                    }
                }
            }

            //MessageBox.Show("Main game sentences are now translated.", "Injection finished...", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region CSV
        private void ParseCsv(string filePath, string fileName, IniParser.Model.IniData iniData, int keyIndex, List<int> listValueIndex)
        {
            iniData.Sections.AddSection(fileName);

            var csv = new CsvHelper.CsvReader(File.OpenText(filePath));

            while (csv.Read())
            {
                var key = csv.GetField(keyIndex);
                foreach (var valueIndex in listValueIndex)
                {
                    var value = csv.GetField(valueIndex).SurroundWithQuotes();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        if (!string.IsNullOrWhiteSpace(key))
                            iniData.Sections.GetSectionData(fileName).Keys.AddKey(valueIndex + "@" + fileName + "@" + key, value);
                    }
                }
            }
        }
        private void InjectCsv(string filePath, string fileName, IniParser.Model.SectionData sectionData)
        {
            //var fileWriter = File.OpenWrite(filePath + ".new");
            var writer = new StreamWriter(filePath + ".new", false, Encoding.Default);
            var csvWriter = new CsvHelper.CsvWriter(writer);
            StreamReader reader = new StreamReader(filePath, Encoding.Default);
            var csvReader = new CsvHelper.CsvReader(reader);

            while (csvReader.Read())
            {
                if (csvReader.Row == 2)
                {
                    var headers = csvReader.FieldHeaders;
                    foreach (var item in headers)
                    {
                        csvWriter.WriteField(item);
                    }
                    csvWriter.NextRecord();
                }

                var ligne = csvReader.CurrentRecord;

                if (ligne != null)
                {
                    foreach (var sectionKey in sectionData.Keys)
                    {
                        int valueIndex = ExtractCsvValueIndexFromString(sectionKey.KeyName);
                        string key = ExtractKeyFromString(sectionKey.KeyName);

                        if (ligne.Length > valueIndex)
                        {
                            if (ligne.Any(k => k == key))
                            {
                                ligne[valueIndex] = sectionKey.Value.RemoveSurroundedQuotes().SanitizeQuotes();
                                if (this.RemoveSpecialsChars)
                                    ligne[valueIndex] = ligne[valueIndex].DeleteAccentAndSpecialsChar().RemoveDiacritics();
                            }
                        }
                    }

                    foreach (var item in ligne)
                    {
                        csvWriter.WriteField(item);
                    }
                    csvWriter.NextRecord();
                }
            }

            reader.Close();
            reader.Dispose();
            csvReader.Dispose();
            writer.Close();
            writer.Dispose();

            // replace old file
            File.Delete(filePath);
            File.Move(filePath + ".new", filePath);
        }
        #endregion

        #region Ini
        private void ParseIni(string sourceFilePath, string sectionName, IniData iniData, List<string> searchedKeys = null)
        {
            iniData.Sections.AddSection(sectionName);
            if (searchedKeys != null)
                searchedKeys = searchedKeys.Select(s => s.ToLower().Trim()).ToList();

            var stringIniParser = new IniParser.FileIniDataParser();
            stringIniParser.Parser.Configuration.AllowDuplicateKeys = true;
            stringIniParser.Parser.Configuration.SkipInvalidLines = true;
            stringIniParser.Parser.Configuration.CommentRegex = new System.Text.RegularExpressions.Regex("^;.*");

            var strinInidata = stringIniParser.ReadFile(sourceFilePath, Encoding.Default);

            // Recherche dans les sections...
            foreach (var iniSection in strinInidata.Sections)
            {
                // ... toutes les cles ou...
                var iniSectionKeys = iniSection.Keys.Where(k => k.KeyName != "name").ToList();

                // ...les cles demandées
                if (searchedKeys != null)
                    iniSectionKeys = iniSectionKeys.Where(k => searchedKeys.Contains(k.KeyName.ToLower().Trim())).ToList();

                foreach (var stringIniKey in iniSectionKeys)
                {
                    var key = string.Format("{0}@{1}@{2}", iniSection.SectionName, sectionName, stringIniKey.KeyName);
                    var value = stringIniKey.Value.Trim().SurroundWithQuotes();
                    iniData.Sections.GetSectionData(sectionName).Keys.AddKey(key, value);
                }
            }
        }

        private void InjectIni(string sourceFilePath, string sectionName, IniParser.Model.SectionData translatedSectionData)
        {
            var stringIniParser = new IniParser.FileIniDataParser();
            stringIniParser.Parser.Configuration.AllowDuplicateKeys = true;
            stringIniParser.Parser.Configuration.SkipInvalidLines = true;
            stringIniParser.Parser.Configuration.CommentRegex = new System.Text.RegularExpressions.Regex("^;.*");

            var stringInidata = stringIniParser.ReadFile(sourceFilePath, Encoding.Default);

            foreach (var sectionKey in translatedSectionData.Keys)
            {
                var section = sectionKey.KeyName.Split('@').First();
                var key = sectionKey.KeyName.Split('@').Last();
                var value = sectionKey.Value.SanitizeQuotes();
                if (this.RemoveSpecialsChars)
                    value = value.DeleteAccentAndSpecialsChar().RemoveDiacritics();

                if (stringInidata.Sections.Any(s => s.SectionName == section))
                {
                    var iniSectionData = stringInidata.Sections.GetSectionData(section);
                    if (iniSectionData.Keys.Any(k => k.KeyName == key))
                    {
                        iniSectionData.Keys.GetKeyData(key).Value = value;
                    }
                }
            }

            stringIniParser.WriteFile(sourceFilePath, stringInidata, Encoding.Default);
        }
        #endregion

        #region Tools
        public static FileType DetectFileType(string transifexFilePath)
        {
            if (!File.Exists(transifexFilePath))
                return FileType.None;

            var fileType = FileType.None;

            var transifexFileParser = new IniParser.FileIniDataParser();
            var transifexInidata = transifexFileParser.ReadFile(transifexFilePath, Encoding.UTF8);

            if (transifexInidata.Sections.Select(s => s.SectionName).Contains("strings.ini"))
            {
                fileType = FileType.MainSentences;
            }
            //else if (!transifexInidata.Sections.Select(s => s.SectionName).Contains("strings.ini") && !transifexInidata.Sections.Select(s => s.SectionName).Contains("mods") && transifexInidata.Sections.Select(s => s.SectionName).Contains("tutorial.csv"))
            //{
            //    fileType = FileType.Titles;
            //}
            else if (transifexInidata.Sections.Select(s => s.SectionName).Contains("mods"))
            {
                fileType = FileType.Mods;
            }

            return fileType;
        }
        private string ExtractKeyFromString(string value)
        {
            var parsed = value.Split('@');
            return parsed.Last();
        }
        private int ExtractCsvValueIndexFromString(string value)
        {
            var parsed = value.Split('@');
            return Convert.ToInt32(parsed.First());
        }
        #endregion
    }
}
