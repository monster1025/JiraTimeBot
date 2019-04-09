using System;

namespace JiraTimeBot.Core.Configuration
{
    public class Settings
    {
        public string JiraUrl = "https://myteam.aeroclub.ru";

        public int MinuterPerWorkDay = 8 * 60;
        
        public TimeSpan ActivationTime { get; set; }
        
        public bool AddCommentsToWorklog { get; set; }
        
        public string JiraPassword { get; set; }
        
        public string JiraQuery { get; set; }
        
        public string JiraUserName { get; set; }
        
        public string MercurialAuthorEmail { get; set; }
        
        public string RepositoryPath { get; set; }
        
        public int RoundToMinutes { get; set; }
        
        public string TimeControlTask { get; set; }
        
        public WorkType WorkType { get; set; }
    }
}
