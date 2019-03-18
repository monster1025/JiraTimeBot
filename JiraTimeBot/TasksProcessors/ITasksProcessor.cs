using System;
using System.Collections.Generic;
using JiraTimeBotForm.Configuration;
using JiraTimeBotForm.TaskTime.Objects;

namespace JiraTimeBotForm.TasksProcessors
{
    interface ITasksProcessor
    {
        void Process(DateTime date, List<TaskTimeItem> taskTimes, Settings settings, bool dummyMode);
    }
}
