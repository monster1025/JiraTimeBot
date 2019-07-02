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
        public CommitType Type { get; set; }

        public TaskTimeItem(string branch, string description, int commits, TimeSpan timeSpent, DateTime startTime, 
                            int filesAffected, CommitType type)
        {
            Branch = branch;
            Description = description;
            Commits = commits;
            TimeSpent = timeSpent;
            StartTime = startTime;
            FilesAffected = filesAffected;
            Type = type;
        }
    }
}
