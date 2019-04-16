using System;
using JiraTimeBot.Core.Configuration;

namespace JiraTimeBot.Ui.Models
{
    //Proxy model of settings
    public class SettingsModel : IModel
    {
        private readonly Settings _settings;

        /// <inheritdoc />
        public SettingsModel(Settings settings)
        {
            _settings = settings;
        }

        public Settings Settings => _settings;

        public string JiraUrl
        {
            get => _settings.JiraUrl;
            set => _settings.JiraUrl = value;
        }

        public int MinuterPerWorkDay
        {
            get => _settings.MinuterPerWorkDay;
            set => _settings.MinuterPerWorkDay = value;
        }

        public TimeSpan ActivationTime
        {
            get => _settings.ActivationTime;
            set => _settings.ActivationTime = value;
        }

        public bool AddCommentsToWorklog
        {
            get => _settings.AddCommentsToWorklog;
            set => _settings.AddCommentsToWorklog = value;
        }

        public string JiraPassword
        {
            get => _settings.JiraPassword;
            set => _settings.JiraPassword = value;
        }

        public string JiraQuery
        {
            get => _settings.JiraPassword;
            set => _settings.JiraQuery = value;
        }

        public string JiraUserName
        {
            get => _settings.JiraUserName;
            set => _settings.JiraUserName = value;
        }

        public string MercurialAuthorEmail
        {
            get => _settings.MercurialAuthorEmail;
            set => _settings.MercurialAuthorEmail = value;
        }

        public string RepositoryPath
        {
            get => _settings.RepositoryPath;
            set => _settings.RepositoryPath = value;
        }

        public int RoundToMinutes
        {
            get => _settings.RoundToMinutes;
            set => _settings.RoundToMinutes = value;
        }

        public string TimeControlTask
        {
            get => _settings.TimeControlTask;
            set => _settings.TimeControlTask = value;
        }

        public WorkType WorkType
        {
            get => _settings.WorkType;
            set => _settings.WorkType = value;
        }
    }
}