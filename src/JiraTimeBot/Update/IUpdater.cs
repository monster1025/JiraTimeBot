namespace JiraTimeBot.Update
{
    internal interface IUpdater
    {
        bool UpdateToNewVersion(bool firstTime);
        void SelfRestart();
    }
}
