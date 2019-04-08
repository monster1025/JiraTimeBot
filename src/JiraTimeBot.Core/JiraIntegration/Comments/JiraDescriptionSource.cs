using System;
using System.Collections.Generic;
using System.Linq;
using JiraTimeBot.Core.Configuration;
using JiraTimeBot.Core.TaskTime.Objects;

namespace JiraTimeBot.Core.JiraIntegration.Comments
{
    public class JiraDescriptionSource : IJiraDescriptionSource
    {
        private readonly List<string> _dummyComments = new List<string>
        {
            "Написание кода", 
            "написание кода", 
            "программирование", 
            "реализация задачи", 
            "кодинг", 
            "код + тесты", 
            "кодинг", 
            "написание кода и тестов"
        };

        public string GetDescription(TaskTimeItem taskTimeItem, bool addCommentsToWorklog, Settings settings)
        {
            if (addCommentsToWorklog)
            {
                return taskTimeItem.Description;
            }

            if (settings.WorkType == WorkType.Mercurial)
            {
                return _dummyComments.OrderBy(f => Guid.NewGuid()).FirstOrDefault();
            }

            return "";
        }
    }
}