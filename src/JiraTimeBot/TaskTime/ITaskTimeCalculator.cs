using System.Collections.Generic;
using System.Threading;
using JiraTimeBotForm.Configuration;
using JiraTimeBotForm.Mercurial;
using JiraTimeBotForm.Mercurial.Objects;
using JiraTimeBotForm.TaskTime.Objects;

namespace JiraTimeBotForm.TaskTime
{
    public interface ITaskTimeCalculator
    {
        List<TaskTimeItem> CalculateTaskTime(List<MercurialCommitItem> commitItems, Settings settings, CancellationToken cancellationToken = default(CancellationToken));
    }
}