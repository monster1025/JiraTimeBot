using System;

namespace JiraTimeBot.Core.TaskTime.Objects
{
    public class TaskTimeItem
    {
        public string Description { get; set; }
        public string Branch { get; set; }
        public TimeSpan Time { get; set; }
        public int Commits { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
