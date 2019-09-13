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
        public string Project { get; set; }

        public TaskTimeItem(string branch, string description, string project, DateTime startTime, TimeSpan timeSpent, int commits, 
                            int filesAffected = 1, CommitType type = CommitType.Task)
        {
            Branch = branch;
            Description = description;
            Project = project;
            Commits = commits;
            TimeSpent = timeSpent;
            StartTime = startTime;
            FilesAffected = filesAffected;
            Type = type;
        }

        public void MergeWith(TaskTimeItem toMerge)
        {
            Commits += toMerge.Commits;
            TimeSpent += toMerge.TimeSpent;
            Description += "\r\n" + toMerge.Description;
            Type |= toMerge.Type;
        }
    }
}
