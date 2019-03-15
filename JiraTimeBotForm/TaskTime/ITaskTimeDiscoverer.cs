using System.Collections.Generic;
using System.Threading;
using JiraTimeBotForm.Configuration;
using JiraTimeBotForm.Mercurial;
using JiraTimeBotForm.Mercurial.Objects;
using JiraTimeBotForm.TaskTime.Objects;

namespace JiraTimeBotForm.TaskTime
{
    public interface ITaskTimeDiscoverer
    {
        List<TaskTimeItem> GetTaskTimes(Settings settings, List<MercurialCommitItem> commitItems, CancellationToken cancellationToken = default(CancellationToken));
    }
}