using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using JiraTimeBot.Core.Encryption;
using Newtonsoft.Json;

namespace JiraTimeBot.Core.Configuration
{
    public class SettingsManager : ISettingsManager
    {
        private static string _settingsFileName = "settings.json";

        public Settings Load()
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


        public Settings LoadAndCheck(Action settingsSource, Action<string> errorMessageReporter)
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

        public void Save(Settings settings)
        {
            var password = new PasswordEncryptionClass().Encrypt(settings.JiraUserName, settings.JiraPassword, settings.JiraUrl);
            settings.JiraPassword = password;

            var configDir = Application.UserAppDataPath.Replace(Application.ProductVersion, "");
            var settingsPath = Path.Combine(configDir, _settingsFileName);
            var settingsString = JsonConvert.SerializeObject(settings);
            File.WriteAllText(settingsPath, settingsString);
        }
        public bool SettingsIsValid(Settings settings, out string message)
        {
            if (settings == null)
            {
                message = "Заполните настройки";
                return false;
            }

            var errors = new List<string>();
            message = null;

            if (settings.RoundToMinutes == default(int))
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

    }
}