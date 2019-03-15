using System;
using System.Collections.Generic;
using System.Threading;
using JiraTimeBotForm.Configuration;

namespace JiraTimeBotForm.Mercurial
{
    public interface IMercurialLog
    {
        List<MercurialCommitItem> GetMercurialLog(Settings settings, DateTime? date = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}