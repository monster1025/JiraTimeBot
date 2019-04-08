using System;
using System.Collections.Generic;
using System.Threading;
using JiraTimeBot.Core.Configuration;
using JiraTimeBot.Core.TaskTime.Objects;

namespace JiraTimeBot.Core.TasksProcessors
{
    public interface ITasksProcessor
    {
        void Process(DateTime setForDate, DateTime realDate, List<TaskTimeItem> taskTimes, Settings settings, bool dummyMode, CancellationToken cancellationToken = default(CancellationToken));
    }
}
