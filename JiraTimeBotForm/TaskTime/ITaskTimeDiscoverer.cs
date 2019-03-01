using System;
using System.Collections.Generic;
using JiraTimeBotForm.Configuration;

namespace JiraTimeBotForm.TaskTime
{
    public interface ITaskTimeDiscoverer
    {
        List<TaskTimeItem> GetTaskTimes(Settings settings, DateTime? date = null);
    }
}