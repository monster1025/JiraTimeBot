using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JiraTimeBotForm.Update
{
    class UpdateChecker
    {
        private const string GithubUser = "monster1025";
        private const string GithubRepo = "JiraTimeBot";

        public string GetLatestVersion(GitHubReleaseResponse release)
        {
            var version = release?.TagName?.TrimStart('v');
            return version;
        }

        public string GetDownloadUrl(GitHubReleaseResponse release)
        {
            var downloadUrl = release?.Assets?.FirstOrDefault()?.BrowserDownloadUrl?.ToString();
            return downloadUrl ?? "";
        }

        public GitHubReleaseResponse GetRelease()
        {
            var latestReleasePage = Get($"https://api.github.com/repos/{GithubUser}/{GithubRepo}/releases/latest");
            if (string.IsNullOrEmpty(latestReleasePage))
            {
                return null;
            }

            var release = JsonConvert.DeserializeObject<GitHubReleaseResponse>(latestReleasePage);
            return release;
        }

        public bool DownloadFile(string url)
        {
            try
            {

                using (var client = new WebClient())
                {
                    client.Headers.Add("User-Agent",
                        "Mozilla/5.0 (X11; Linux i686; rv:64.0) Gecko/20100101 Firefox/64.0");
                    client.DownloadFile(url, "update.zip");
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string Get(string uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.UserAgent = "Mozilla/5.0 (X11; Linux i686; rv:64.0) Gecko/20100101 Firefox/64.0";

                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
