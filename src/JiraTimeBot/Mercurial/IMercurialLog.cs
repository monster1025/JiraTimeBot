using System;
using System.Collections.Generic;
using System.Threading;
using JiraTimeBot.Configuration;
using JiraTimeBot.TaskTime.Objects;

namespace JiraTimeBot.Mercurial
{
    public interface IMercurialLog
    {
        List<TaskTimeItem> GetMercurialLog(Settings settings, DateTime? date = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}