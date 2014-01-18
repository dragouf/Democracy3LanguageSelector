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
    public class TransifexConnector
    {
        private const string BaseUrl = "https://www.transifex.com/api/2";
        private const string ProjectSlug = "democracy-3";

        public async Task<Project> ProjectDetails()
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

        public async Task<TranslationDetails> TranslationDetails(string langCode)
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

            return langDetails;
        }

        public async Task DownloadTranslationResources(string langCode, List<Resource> resourceList, IProgress<int> progress)
        {
            //Parallel.ForEach(resourceList, resource =>
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

        public async Task<int> GetTotalResouresSize(string langCode, List<Resource> resourceList)
        {
            int total = 0;

            //Parallel.ForEach(resourceList, resource =>   
            foreach (var resource in resourceList)
            {
                var request = "{0}/project/{1}/resource/{2}/translation/{3}/?file".FormatWith(BaseUrl, ProjectSlug, resource.Slug, langCode);

                System.Net.WebRequest client = System.Net.HttpWebRequest.Create(request);

                client.Credentials = GetCrendentials();
                client.PreAuthenticate = true;

                using (System.Net.WebResponse resp = await client.GetResponseAsync())
                {
                    total += (int)resp.ContentLength;
                    resp.Close();
                }
            }

            return total;
        }

        private ICredentials GetCrendentials()
        {
            return new NetworkCredential("lili22@yopmail.com", "ilil22");
        }
    }
}
