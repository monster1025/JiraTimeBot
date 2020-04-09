using System;

namespace JiraTimeBot.TaskTime.Objects
{
    [Flags]
    public enum CommitType
    {
        Task = 1,
        Release = 2
    }
}