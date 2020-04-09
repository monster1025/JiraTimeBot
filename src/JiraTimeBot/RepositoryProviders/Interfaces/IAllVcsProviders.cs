using JiraTimeBot.JiraIntegration;

namespace JiraTimeBot.RepositoryProviders.Interfaces
{
    public interface IAllRepositoryProviders
    {
        MercurialLog MercurialLog { get; }
        GitLog GitLog { get; }
        JiraCommitEmulator JiraCommitEmulator { get; }
        IJiraHistory JiraHistory { get; }
    }
}
