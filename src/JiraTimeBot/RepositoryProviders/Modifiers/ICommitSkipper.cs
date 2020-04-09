namespace JiraTimeBot.RepositoryProviders.Modifiers
{
    public interface ICommitSkipper
    {
        bool IsNeedToSkip(string branch, string commitMessage);
    }
}
