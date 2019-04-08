using System.Collections.Generic;
using System.Threading;
using JiraTimeBot.Core.Configuration;
using JiraTimeBot.Core.Mercurial.Objects;
using JiraTimeBot.Core.TaskTime.Objects;

namespace JiraTimeBot.Core.TaskTime
{
    public interface ITaskTimeCalculator
    {
        List<TaskTimeItem> CalculateTaskTime(List<MercurialCommitItem> commitItems, 
                                             Settings settings, 
                                             CancellationToken cancellationToken = default);
    }
}