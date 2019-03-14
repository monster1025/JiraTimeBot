using System;
using System.Collections.Generic;
using JiraTimeBotForm.Configuration;
using JiraTimeBotForm.TaskTime;

namespace JiraTimeBotForm.TaskProcessors
{
    interface ITasksProcessor
    {
        void Process(DateTime date, List<TaskTimeItem> taskTimes, Settings settings);
    }
}
