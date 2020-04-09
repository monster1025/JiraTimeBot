using JiraTimeBot.Configuration;
using JiraTimeBot.TaskTime.Objects;
using System.Collections.Generic;
using System.Threading;

namespace JiraTimeBot.TaskTime
{
    public interface ITaskTimeCalculator
    {
        List<TaskTimeItem> CalculateTaskTime(List<TaskTimeItem> commitItems, Settings settings, CancellationToken cancellationToken = default(CancellationToken));
    }
}