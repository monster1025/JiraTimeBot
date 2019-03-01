using System;

namespace JiraTimeBotForm.TaskTime
{
    public class TaskTimeItem
    {
        public string Branch { get; set; }
        public TimeSpan Time { get; set; }
        public int Commits { get; set; }
    }
}
