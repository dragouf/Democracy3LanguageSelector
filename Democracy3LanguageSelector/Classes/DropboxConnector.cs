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
    public static class DropboxConnector
    {
        private const string BaseUrl = "https://dl.dropboxusercontent.com/u/260255445/Democracy%203/cache";

        public static async Task<DateTime> GetBackupDate()
        {
            string responseText = string.Empty;

            var request = "{0}/last_update".FormatWith(BaseUrl);
            var client = System.Net.HttpWebRequest.Create(request);
            var wr = await client.GetResponseAsync();

            using (var stream = wr.GetResponseStream())
            {
                using (var reader = new System.IO.StreamReader(stream))
                {
                    responseText = await reader.ReadToEndAsync();
                }
            }

            wr.Close();

            var date = DateTime.Now;
            DateTime.TryParse(responseText, out date);

            return date;
        }
      
        public static async Task DownloadTranslationResources(string langCode, List<Resource> resourceList, IProgress<int> progress)
        {
            foreach (var resource in resourceList)
            {
                var request = "{0}/{1}/{2}.ini".FormatWith(BaseUrl, langCode, resource.Slug);

                var client = HttpWebRequest.Create(request);
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
    }
}
