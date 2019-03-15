using System;
using System.Collections.Generic;
using System.Linq;
using JiraTimeBotForm.TaskTime;

namespace JiraTimeBotForm.JiraIntegration
{
    public class JiraDescriptionSource : IJiraDescriptionSource
    {
        private readonly List<string> _dummyComments = new List<string>
        {
            "Написание кода", "написание кода", "программирование", "реализация задачи", "кодинг", "код + тесты", 
            "кодинг", "написание кода и тестов"
        };

        public string GetDescription(TaskTimeItem taskTimeItem, bool addCommentsToWorklog)
        {
            var comment = _dummyComments.OrderBy(f=>Guid.NewGuid()).FirstOrDefault();
            if (addCommentsToWorklog)
            {
                comment = taskTimeItem.Description;
            }

            return comment;
        }
    }
}