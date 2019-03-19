using System;
using System.Collections.Generic;
using JiraTimeBot.Configuration;
using JiraTimeBot.TaskTime.Objects;

namespace JiraTimeBot.TasksProcessors
{
    interface ITasksProcessor
    {
        void Process(DateTime date, List<TaskTimeItem> taskTimes, Settings settings, bool dummyMode);
    }
}
