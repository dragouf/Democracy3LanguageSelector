using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Democracy3LanguageSelector
{
    public static class TransifexConnector
    {
        private const string BaseUrl = "https://www.transifex.com/api/2";
        private const string ProjectSlug = "democracy-3";

        public static async Task<Project> ProjectDetails()
        {
            var request = "{0}/project/{1}/?details".FormatWith(BaseUrl, ProjectSlug);

            var client = HttpWebRequest.Create(request);
            client.Credentials = GetCrendentials(); 
            client.PreAuthenticate = true;

            var wr = await client.GetResponseAsync();

            string jsonResult = string.Empty;

            using (var sr = new StreamReader(wr.GetResponseStream()))
            {
                jsonResult = await sr.ReadToEndAsync();
            }

            var model = await JsonConvert.DeserializeObjectAsync<Project>(jsonResult);

            // Always add source language
            model.Teams.Insert(0, "en");

            return model;
        }

        public static async Task<TranslationDetails> TranslationDetails(string langCode)
        {
            bool isEnglish = false;
            if (langCode == "en")
            {
                langCode = "fr";
                isEnglish = true;
            }

            var request = "{0}/project/{1}/language/{2}/?details".FormatWith(BaseUrl, ProjectSlug, langCode);

            var client = HttpWebRequest.Create(request);
            client.Credentials = GetCrendentials();
            client.PreAuthenticate = true;

            var wr = await client.GetResponseAsync();

            string jsonResult = string.Empty;

            using (var sr = new StreamReader(wr.GetResponseStream()))
            {
                jsonResult = await sr.ReadToEndAsync();
            }

            var langDetails = await JsonConvert.DeserializeObjectAsync<TranslationDetails>(jsonResult);

            if (isEnglish)
            {
                langDetails.Translated_segments = langDetails.Total_segments;
            }

            // Write progression to data file
            WriteDataFile(isEnglish ? "en" : langCode, langDetails.Translated_segments);

            return langDetails;
        }

        public static async Task DownloadTranslationResources(string langCode, List<Resource> resourceList, IProgress<int> progress)
        {
            foreach (var resource in resourceList)
            {
                var request = "{0}/project/{1}/resource/{2}/translation/{3}/?file".FormatWith(BaseUrl, ProjectSlug, resource.Slug, langCode);

                var client = HttpWebRequest.Create(request);
                client.Credentials = GetCrendentials();
                client.PreAuthenticate = true;

                var wr = await client.GetResponseAsync();

                using (var stream = wr.GetResponseStream())
                {
                    byte[] buffer = new byte[1024];
                    string filePath = @"cache\{0}\{1}.ini".FormatWith(langCode, resource.Slug);
                    using (FileStream outFile = new FileStream(filePath, FileMode.Create))
                    {
                        int bytesRead;
                        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                        {
                            await outFile.WriteAsync(buffer, 0, bytesRead);
                            progress.Report(buffer.Length);
                        }

                        outFile.Close();
                    }
                }

                wr.Close();
            }
        }

        public static int GetTotalResouresSize(string langCode, List<Resource> resourceList, int totalStrings)
        {
            int total = 165 * totalStrings;

            return total;
        }

        public static void WriteDataFile(string langCode, int translatedStrings)
        {
            File.WriteAllText("cache\\{0}\\data".FormatWith(langCode), translatedStrings.ToString());
        }

        public static int GetLastProgressionFromDataFile(string langCode)
        {
            int total = 0;

            if (File.Exists("cache\\{0}\\data".FormatWith(langCode)))
            {
                Int32.TryParse(File.ReadAllText("cache\\{0}\\data".FormatWith(langCode)).Trim(), out total);
            }

            return total;
        }

        private static ICredentials GetCrendentials()
        {
            // I know it's not secure but it's just not clear^^
            return new NetworkCredential(
                Secure.Decrypt("UevGaasLQTXt9DtBfSzNQEaRAWNulzJEF7yhUqJg3uk="),
                Secure.Decrypt("G0TfnocewaZATteVQ6Cj7g==")
            );
        }
    }
}
