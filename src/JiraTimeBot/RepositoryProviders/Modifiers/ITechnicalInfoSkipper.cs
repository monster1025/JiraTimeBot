namespace JiraTimeBot.RepositoryProviders.Modifiers
{
    public interface ITechnicalInfoSkipper
    {
        string StripBranchPrefix(string branch, string commitMessage);
        string StripTechnicalInfo(string commitMessage);
    }
}
