namespace JiraTimeBot.Core.Update
{
    public interface IUpdater
    {
        bool UpdateToNewVersion(bool firstTime);
    }
}
