using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JiraTimeBotForm.CommitWorks
{
    public class BuzzwordReplacer
    {
        public string FixBuzzwords(string commitMessage)
        {
            Dictionary<string, string> buzzReplace = new Dictionary<string, string>()
            {
                {"fix", "исправление"},
                {"up", "обновил"},
                {"init", "начало"},
                {"build", "сборка"},
                {"namespace", "неймспейс"}
            };

            foreach (var buzzReplaceItm in buzzReplace)
            {
                commitMessage = ReplaceCaseInsensitive(commitMessage, $" {buzzReplaceItm.Key} ", $" {buzzReplaceItm.Value} ");
                commitMessage = ReplaceCaseInsensitive(commitMessage, $" {buzzReplaceItm.Key}.", $" {buzzReplaceItm.Value}.");
                commitMessage = ReplaceCaseInsensitive(commitMessage, $" {buzzReplaceItm.Key},", $" {buzzReplaceItm.Value},");
                if (commitMessage.StartsWith($"{buzzReplaceItm.Key}", StringComparison.InvariantCultureIgnoreCase))
                {
                    commitMessage = ReplaceCaseInsensitive(commitMessage, $"{buzzReplaceItm.Key} ", $"{buzzReplaceItm.Value} ");
                }
            }

            return commitMessage;
        }

        private string ReplaceCaseInsensitive(string input, string search, string replacement){
            string result = Regex.Replace(
                    input,
                    Regex.Escape(search), 
                    replacement.Replace("$","$$"), 
                    RegexOptions.IgnoreCase
                );
            return result;
        }

    }
}
