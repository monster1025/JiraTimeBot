using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace JiraTimeBot.Configuration
{
    public class Settings
    {
        public int MinuterPerWorkDay { get; set; }
        public int RoundToMinutes { get; set; }

        public string JiraUrl = "https://myteam.aeroclub.ru";
        public bool PullBeforeProcess { get; set; }
        public string RepositoryPath { get; set; }
        public string MercurialAuthorEmail { get; set; }
        public string JiraUserName { get; set; }
        public string JiraPassword { get; set; }
        public TimeSpan ActivationTime { get; set; }
        public bool AddCommentsToWorklog { get; set; }
        public WorkType WorkType { get; set; }
        public string JiraQuery { get; set; }
        public string TimeControlTask { get; set; }
        public int RandomWorkMinutes { get; set; }

        public static Settings LoadAndCheck(Action settingsSource, Action<string> errorMessageReporter)
        {
            Settings settings;
            bool settingsValid;

            do
            {
                settings = Load();
                settingsValid = SettingsIsValid(settings, out var message);

                if (!settingsValid)
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        errorMessageReporter(message);
                    }

                    settingsSource();
                }

            } while (!settingsValid);
            
            return settings;
        }

        private static bool SettingsIsValid(Settings settings, out string message)
        {
            if (settings == null)
            {
                message = "Заполните настройки";
                return false;
            }

            var errors = new List<string>();
            message = null;

            if (settings.RoundToMinutes == default)
            {
                errors.Add("Укажите округление времени, например, 10 (минут)");
            }

            if (string.IsNullOrEmpty(settings.JiraPassword))
            {
                errors.Add("Укажите Password от Jira");
            }

            if (string.IsNullOrEmpty(settings.JiraUserName))
            {
                errors.Add("Укажите UserName от Jira");
            }

            if (settings.WorkType == WorkType.Mercurial && (string.IsNullOrEmpty(settings.RepositoryPath) || !Directory.Exists(settings.RepositoryPath)))
            {
                errors.Add("Укажите верный RepositoryPath");
            }

            if (!string.IsNullOrEmpty(settings.TimeControlTask) && !settings.TimeControlTask.Contains("-"))
            {
                errors.Add("Укажите верно зажачу контроля времени. Она джолжна содержать знак '-', например, SV-1211");
            }

            var hasErrors = errors.Any();
            if (hasErrors)
            {
                message = string.Join(Environment.NewLine, errors);
            }

            return !hasErrors;
        }

        public static Settings Load()
        {
            var configDir = Application.UserAppDataPath.Replace(Application.ProductVersion, "");
            var settingsPath = Path.Combine(configDir, _settingsFileName);
            if (!File.Exists(settingsPath))
            {
                return null;
            }

            var settingsText = File.ReadAllText(settingsPath);

            Settings settings;

            try
            {
                settings = JsonConvert.DeserializeObject<Settings>(settingsText);
                if (settings == null)
                {
                    return null;
                }
                var password = new PasswordEncryptionClass().Decrypt(settings.JiraUserName, settings.JiraPassword, settings.JiraUrl);
                settings.JiraPassword = password;
            }
            catch (JsonReaderException)
            {
                return null;
            }

            return settings;
        }

        public void Save()
        {
            var password = new PasswordEncryptionClass().Encrypt(this.JiraUserName, this.JiraPassword, this.JiraUrl);
            this.JiraPassword = password;

            var configDir = Application.UserAppDataPath.Replace(Application.ProductVersion, "");
            var settingsPath = Path.Combine(configDir, _settingsFileName);
            var settingsString = JsonConvert.SerializeObject(this);
            File.WriteAllText(settingsPath, settingsString);
        }

        private static readonly string _settingsFileName = "settings.json";
    }

    public enum WorkType
    {
        Mercurial = 0,
        JiraLogs = 1,
    }
}
