using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace JiraTimeBotForm.Configuration
{
    public class Settings
    {
        public int MinuterPerWorkDay = 8 * 60;
        public int RountToMinutes { get; set; }

        public string JiraUrl = "https://myteam.aeroclub.ru";

        public string RepositoryPath { get; set; }
        public string MercurialAuthorEmail { get; set; }
        public string JiraUserName { get; set; }
        public string JiraPassword { get; set; }
        public TimeSpan ActivationTime { get; set; }
        public bool DummyMode { get; set; }
        public bool AddCommentsToWorklog { get; set; }


        public static Settings Load()
        {
            var settingsPath = Path.Combine(Application.UserAppDataPath, _settingsFileName);
            if (!File.Exists(settingsPath))
            {
                return null;
            }

            var settingsText = File.ReadAllText(settingsPath);
            var settings = JsonConvert.DeserializeObject<Settings>(settingsText);

            var password = new PasswordEncryptionClass().Decrypt(settings.JiraUserName, settings.JiraPassword, settings.JiraUrl);
            settings.JiraPassword = password;
            return settings;
        }

        public void Save()
        {
            var password = new PasswordEncryptionClass().Encrypt(this.JiraUserName, this.JiraPassword, this.JiraUrl);
            this.JiraPassword = password;

            var settingsPath = Path.Combine(Application.UserAppDataPath, _settingsFileName);
            var settingsString = JsonConvert.SerializeObject(this);
            File.WriteAllText(settingsPath, settingsString);
        }

        private static string _settingsFileName = "settings.json";
    }
}
