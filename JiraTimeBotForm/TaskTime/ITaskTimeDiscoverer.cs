using System.Collections.Generic;
using System.Threading;
using JiraTimeBotForm.Configuration;
using JiraTimeBotForm.Mercurial;

namespace JiraTimeBotForm.TaskTime
{
    public interface ITaskTimeDiscoverer
    {
        List<TaskTimeItem> GetTaskTimes(Settings settings, List<MercurialCommitItem> commitItems, CancellationToken cancellationToken = default(CancellationToken));
    }
}