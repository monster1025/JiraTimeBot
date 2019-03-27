using System;
using System.Collections.Generic;
using System.Threading;
using JiraTimeBot.Configuration;
using JiraTimeBot.TaskTime.Objects;

namespace JiraTimeBot.TasksProcessors
{
    interface ITasksProcessor
    {
        void Process(DateTime setForDate, DateTime realDate, List<TaskTimeItem> taskTimes, Settings settings, bool dummyMode, CancellationToken cancellationToken = default(CancellationToken));
    }
}
