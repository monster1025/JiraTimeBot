using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiraTimeBotForm.JiraIntegration;

namespace JiraTimeBotForm.Mercurial
{
    public interface IAllMercurialProviders
    {
        MercurialLog MercurialLog { get; }
        JiraCommitEmulator JiraCommitEmulator { get; }
    }
}
