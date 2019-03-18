using System;
using System.Collections.Generic;
using System.Threading;
using JiraTimeBotForm.Configuration;
using JiraTimeBotForm.Mercurial.Objects;

namespace JiraTimeBotForm.Mercurial
{
    public interface IMercurialLog
    {
        List<MercurialCommitItem> GetMercurialLog(Settings settings, DateTime? date = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}