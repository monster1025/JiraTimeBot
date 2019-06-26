﻿using System;
using System.Collections.Generic;
using System.Threading;
using JiraTimeBot.Configuration;
using JiraTimeBot.Mercurial.Objects;

namespace JiraTimeBot.Mercurial
{
    public interface IMercurialLog
    {
        List<MercurialCommitItem> GetMercurialLog(Settings settings, DateTime? date = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}