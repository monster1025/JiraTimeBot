using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace JiraTimeBotForm
{
    public class Logger : ILog
    {
        private readonly TextBox _txt;

        public Logger(TextBox txt)
        {
            _txt = txt;
        }

        private void AppendText(string message)
        {
            message = $"{DateTime.Now:HH:mm} {message}";

            if (string.IsNullOrEmpty(_txt.Text))
            {
                _txt.Text = message;
                return;
            }

            _txt.Text += Environment.NewLine + message;
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

    public interface ILog
    {
        void Info([Localizable(false)] string message);
        void Trace([Localizable(false)] string message);
        void Warn([Localizable(false)] string message);
        void Error([Localizable(false)] string message);
    }
}
