using System;

namespace JiraTimeBot.TaskTime.Objects
{
    public class TaskTimeItem
    {
        public string Description { get; set; }
        public string Branch { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan TimeSpent { get; set; }
        public int Commits { get; set; }
        public int FilesAffected { get; set; }
    }
}
