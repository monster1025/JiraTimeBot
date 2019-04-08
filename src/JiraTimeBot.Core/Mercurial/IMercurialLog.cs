using System;
using System.Collections.Generic;
using System.Threading;
using JiraTimeBot.Core.Configuration;
using JiraTimeBot.Core.Mercurial.Objects;

namespace JiraTimeBot.Core.Mercurial
{
    public interface IMercurialLog
    {
        List<MercurialCommitItem> GetMercurialLog(Settings settings, DateTime? date = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}