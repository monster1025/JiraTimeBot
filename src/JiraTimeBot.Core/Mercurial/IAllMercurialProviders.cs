using JiraTimeBot.Core.JiraIntegration;

namespace JiraTimeBot.Core.Mercurial
{
    public interface IAllMercurialProviders
    {
        MercurialLog MercurialLog { get; }
        JiraCommitEmulator JiraCommitEmulator { get; }
    }
}
