using System.Collections.Generic;
using System.Threading;
using JiraTimeBot.Configuration;
using JiraTimeBot.Mercurial.Objects;
using JiraTimeBot.TaskTime.Objects;

namespace JiraTimeBot.TaskTime
{
    public interface ITaskTimeCalculator
    {
        List<TaskTimeItem> CalculateTaskTime(List<MercurialCommitItem> commitItems, Settings settings, CancellationToken cancellationToken = default(CancellationToken));
    }
}