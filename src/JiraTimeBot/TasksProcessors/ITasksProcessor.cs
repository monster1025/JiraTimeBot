using JiraTimeBot.Configuration;
using JiraTimeBot.TaskTime.Objects;
using System;
using System.Collections.Generic;
using System.Threading;

namespace JiraTimeBot.TasksProcessors
{
    interface ITasksProcessor
    {
        void Process(DateTime setForDate, 
                     DateTime realDate, 
                     List<TaskTimeItem> taskTimes, 
                     Settings settings, 
                     bool dummyMode, 
                     CancellationToken cancellationToken = default);
    }
}
