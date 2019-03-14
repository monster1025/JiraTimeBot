using System;
using System.Collections.Generic;
using System.Threading;
using JiraTimeBotForm.Configuration;

namespace JiraTimeBotForm.TaskTime
{
    public interface ITaskTimeDiscoverer
    {
        List<TaskTimeItem> GetTaskTimes(Settings settings, DateTime? date = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}