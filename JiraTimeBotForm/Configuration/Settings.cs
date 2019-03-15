using System;

namespace JiraTimeBotForm.Configuration
{
    public class Settings
    {
        public int MinuterPerWorkDay = 8 * 60;
        public string JiraUrl = "https://myteam.aeroclub.ru";

        public string RepositoryPath { get; set; }
        public string MercurialAuthorEmail { get; set; }
        public string JiraUserName { get; set; }
        public string JiraPassword { get; set; }
        public TimeSpan ActivationTime { get; set; }
        public bool DummyMode { get; set; }
        public bool AddCommentsToWorklog { get; set; }
    }
}
