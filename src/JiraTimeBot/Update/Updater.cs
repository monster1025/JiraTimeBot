using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JiraTimeBot.Update
{
    public class Updater : IUpdater
    {
        private readonly ILog _log;
        private double _downloadPercent = 0;

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

                var appFile = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var bkpDir = Path.Combine(appFile.DirectoryName, "bkp");

                if (Directory.Exists(bkpDir))
                    Directory.Delete(bkpDir, true);
                _log.Info($"Создаю директорию {bkpDir}.");
                Directory.CreateDirectory(bkpDir);

                //забэкапим все папки
                foreach (var dir in Directory.GetDirectories(appFile.DirectoryName).Where(f=>!f.EndsWith("bkp")))
                {
                    var dirName = new DirectoryInfo(dir);
                    var bkpDirPath = Path.Combine(bkpDir, dirName.Name);
                    if (Directory.Exists(bkpDirPath))
                        Directory.Delete(bkpDirPath, true);
                    Directory.Move(dir, bkpDirPath);
                }
                //и файлы
                foreach (var file in Directory.GetFiles(appFile.DirectoryName).Where(f=>!f.EndsWith("update.zip")))
                {
                    File.Move(file, Path.Combine(bkpDir, new FileInfo(file).Name));
                }

                //распакуем архив
                using (ZipArchive archive = ZipFile.OpenRead(Path.Combine(appFile.DirectoryName, updateFile)))
                {
                    archive.ExtractToDirectory(appFile.DirectoryName);
                }

                _log.Info("Обновление установлено. Перезапустите программу для применения изменений.");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"Что-то пошло не так при обновлении {ex.Message}: {ex.StackTrace}.");
                return false;
            }
        }

        public void SelfRestart()
        {
            var name = Application.ExecutablePath;
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
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
                    client.DownloadProgressChanged += ClientOnDownloadProgressChanged;
                    client.DownloadFileAsync(new Uri(url), to);
                    
                    while (client.IsBusy) { Application.DoEvents(); }
                    
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var percent = Math.Truncate(e.BytesReceived / (double)e.TotalBytesToReceive * 100);
            if (Math.Abs(_downloadPercent - percent) >= 10)
            {
                _log.Info($"Downloaded: {percent}%");
                _downloadPercent = percent;
            }
        }

        private string Get(string uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.UserAgent = "Mozilla/5.0 (X11; Linux i686; rv:64.0) Gecko/20100101 Firefox/64.0";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
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