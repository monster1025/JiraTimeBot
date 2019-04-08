using System;
using System.ComponentModel;
using JiraTimeBot.Core;

namespace JiraTimeBot
{
    public class Logger : ILog
    {
        private readonly Action<string> _logAction;

        public Logger(Action<string> logAction)
        {
            _logAction = logAction;
        }

        private void AppendText(string message)
        {
            message = $"{DateTime.Now:HH:mm} {message}";
            _logAction(message);
        }

        public void Info([Localizable(false)] string message)
        {
            AppendText(message);
        }

        public void Trace([Localizable(false)] string message)
        {
            AppendText(message);
        }

        public void Warn([Localizable(false)] string message)
        {
            AppendText(message);
        }

        public void Error([Localizable(false)] string message)
        {
            AppendText(message);
        }
    }
}
