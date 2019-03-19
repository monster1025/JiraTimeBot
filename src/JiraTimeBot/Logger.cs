using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace JiraTimeBot
{
    public class Logger : ILog
    {
        private readonly TextBox _txt;
        private readonly AppendLog _safeAppender;

        public Logger(TextBox txt)
        {
            _txt = txt;
            _safeAppender = text => _txt.AppendText(text);

        }

        private void AppendText(string message)
        {
            message = $"{DateTime.Now:HH:mm} {message}";
            _txt.Invoke(_safeAppender, message + Environment.NewLine);
        }

        private delegate void AppendLog(string text);

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

    public interface ILog
    {
        void Info([Localizable(false)] string message);
        void Trace([Localizable(false)] string message);
        void Warn([Localizable(false)] string message);
        void Error([Localizable(false)] string message);
    }
}
