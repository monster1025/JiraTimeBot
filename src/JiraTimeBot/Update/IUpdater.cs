using System;

namespace JiraTimeBot.Update
{
    internal interface IUpdater
    {
        bool UpdateToNewVersion(bool firstTime);
    }
}
