using System.Collections.Generic;
using System.Threading;
using JiraTimeBot.Configuration;

using JiraTimeBot.TaskTime.Objects;

namespace JiraTimeBot.TaskTime
{
    public interface ITaskTimeCalculator
    {
        List<TaskTimeItem> CalculateTaskTime(List<TaskTimeItem> commitItems, Settings settings, CancellationToken cancellationToken = default(CancellationToken));
    }
}