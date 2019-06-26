using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace JiraTimeBot.Update
{
    public class Updater : IUpdater
    {
        private readonly ILog _log;

        public Updater(ILog log)
        {
            _log = log;
        }

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

        public bool UpdateToNewVersion(bool firstTime)
        {
            var release = GetRelease();
            if (release == null)
            {
                return false;
            }

            var gitVersionStr = GetLatestVersion(release);
            var gitVersion = new Version(gitVersionStr);
            var appVersion = new Version(Application.ProductVersion);

            if (gitVersion > appVersion)
            {
                var downloadUrl = GetDownloadUrl(release);
                _log.Info($"Доступна новая версия: {gitVersion} {downloadUrl}");

                var success = DownloadFile(downloadUrl, "update.zip");
                if (!success)
                {
                    return false;
                }

                var result = InstallUpdate(gitVersion, "update.zip");
                if (result)
                {
                    return true;
                }

                return false;
            }

            if (firstTime)
            {
                _log.Info($"Вы используете актуальную версию.");
            }
            return false;
        }

        public bool InstallUpdate(Version gitVersion, string updateFile)
        {
            try
            {
                _log.Info($"Устанавливаю обновление до версии {gitVersion}.");
                //Rename he executing file
                System.IO.FileInfo appFile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var bkpDir = Path.Combine(appFile.DirectoryName, "bkp");
                if (!Directory.Exists(bkpDir))
                {
                    Directory.CreateDirectory(bkpDir);
                }

                using (ZipArchive archive = ZipFile.OpenRead(Path.Combine(appFile.DirectoryName, updateFile)))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        var zipEntryPath = Path.Combine(appFile.DirectoryName, entry.FullName);
                        if (File.Exists(zipEntryPath))
                        {
                            System.IO.FileInfo zipEntryFile = new System.IO.FileInfo(zipEntryPath);
                            if (File.Exists(bkpDir + "\\" + zipEntryFile.Name))
                            {
                                File.Delete(bkpDir + "\\" + zipEntryFile.Name);
                            }
                            System.IO.File.Move(zipEntryFile.FullName, bkpDir + "\\" + zipEntryFile.Name);
                        }

                        entry.ExtractToFile(zipEntryPath);
                    }
                }

                _log.Info("Обновление установлено. Перезапустите программу для применения изменений.");
                return true;
            }
            catch (Exception)
            {
                _log.Error($"Что-то пошло не так при обновлении.");
                return false;
            }
        }
        public void SelfRestart()
        {
            var name = Application.ExecutablePath;
            ProcessStartInfo startInfo = new ProcessStartInfo {
                Arguments = "/C choice /C Y /N /D Y /T 1 & \"" + name + "\"",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                FileName = "cmd.exe"
            };
            Process.Start(startInfo);
            Application.Exit();
        }


        private bool DownloadFile(string url, string to)
        {
            try
            {

                using (var client = new WebClient())
                {
                    client.Headers.Add("User-Agent",
                        "Mozilla/5.0 (X11; Linux i686; rv:64.0) Gecko/20100101 Firefox/64.0");
                    client.DownloadFile(url, to);
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
            catch (Exception)
            {
                return "";
            }
        }
    }
}