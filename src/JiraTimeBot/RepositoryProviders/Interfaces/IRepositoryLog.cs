using JiraTimeBot.Configuration;
using JiraTimeBot.TaskTime.Objects;
using System;
using System.Collections.Generic;
using System.Threading;

namespace JiraTimeBot.RepositoryProviders.Interfaces
{
    public interface IRepositoryLog
    {
        List<TaskTimeItem> GetRepositoryLog(Settings settings, 
                                            string currentRepository,
                                            DateTime? date = null, 
                                            CancellationToken cancellationToken = default(CancellationToken));
    }
}