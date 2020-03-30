using JiraTimeBot.JiraIntegration;

namespace JiraTimeBot.Mercurial
{
    public interface IAllMercurialProviders
    {
        MercurialLog MercurialLog { get; }
        GitLog GitLog { get; }
        JiraCommitEmulator JiraCommitEmulator { get; }
        IJiraHistory JiraHistory { get; }
    }
}
