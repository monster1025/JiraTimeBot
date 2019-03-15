using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using JiraTimeBotForm.Configuration;
using JiraTimeBotForm.DI;
using JiraTimeBotForm.TasksProcessors;
using JiraTimeBotForm.UI.Tray;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace JiraTimeBotForm.UI
{
    public partial class frmMain : Form
    {
        private readonly ILog _log;
        private readonly Job _job;
        private readonly IReadOnlyList<Control> _controls;
        private readonly IContainer _container;
        private readonly IAllTasksProcessors _tasksProcessors;
        private readonly ITrayMenu _trayIcon;
        private CancellationTokenSource _tokenSource;

        private CancellationTokenSource GetTokenSource()
        {
            var tokenSource = new CancellationTokenSource();
            tokenSource.Token.Register(() =>
            {
                _log.Info("Операция отменена...");
                LockUnlock(true);
            });

            return tokenSource;
        }

        public frmMain()
        {
            InitializeComponent();
            _container = new Bootstrapper().Build(txtLog);

            _trayIcon = _container.Resolve<ITrayMenu>();
            _trayIcon.Create(this);

            _job = _container.Resolve<Job>();
            _log = _container.Resolve<ILog>();
            _tasksProcessors = _container.Resolve<IAllTasksProcessors>();

            _controls = new Control[] { txtJiraLogin, txtJiraPassword, txtMercurialEmail, actTime, txtRepoPath, txtDummyMode, btnSave, btnStart, btnMeeting, chkAddComments };
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _trayIcon.Hide();
        }

        public Settings ReadSettingsAndLock()
        {
            var settings = new Settings
            {
                JiraUserName = txtJiraLogin.Text,
                JiraPassword = txtJiraPassword.Text,
                MercurialAuthorEmail = txtMercurialEmail.Text,
                ActivationTime = TimeSpan.Parse(actTime.Text),
                RepositoryPath = txtRepoPath.Text,
                DummyMode = txtDummyMode.Checked,
                AddCommentsToWorklog = chkAddComments.Checked,
            };
            
            LockUnlock(false);

            return settings;
        }

        public void LockUnlock(bool enabled)
        {
            foreach (var control in _controls)
            {
                control.Enabled = enabled;
            }

            btnCancel.Enabled = !enabled;
            tmrStart.Enabled = enabled;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var settings = ReadSettingsAndLock();
            LockUnlock(true);

            settings.Save();

            MessageBox.Show("Settings saved.");
            settings = Settings.Load();
            if (settings != null)
            {
                SetSettings(settings);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var settings = Settings.Load();
            if (settings != null)
            {
                SetSettings(settings);
            }
        }

        private void SetSettings(Settings settings)
        {
            txtJiraLogin.Text = settings.JiraUserName;
            txtJiraPassword.Text = settings.JiraPassword;
            txtMercurialEmail.Text = settings.MercurialAuthorEmail;
            txtRepoPath.Text = settings.RepositoryPath;
            actTime.Text = settings.ActivationTime.ToString("hh\\:mm\\:ss");
            txtDummyMode.Checked = settings.DummyMode;
            chkAddComments.Checked = settings.AddCommentsToWorklog;
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            var settings = ReadSettingsAndLock();

            if (!Directory.Exists(settings.RepositoryPath))
            {
                MessageBox.Show("Папка с репо не сушествует.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (_tokenSource = GetTokenSource())
            {
                await _job.DoTheJob(settings, _tasksProcessors.WorkLogTasksProcessor, _tokenSource.Token); 
            }

            LockUnlock(true);
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                _trayIcon.Show();
                this.Hide();
            }

            else if (FormWindowState.Normal == this.WindowState)
            {
                _trayIcon.Hide();
            }
        }

        private async void tmrStart_Tick(object sender, EventArgs e)
        {
            var runTime = TimeSpan.Parse(actTime.Text);

            if (Math.Abs((DateTime.Now.TimeOfDay - runTime).TotalSeconds) < 1)
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    _log.Error("Сегодня суббота или воскресенье. Работать нельзя =)");
                    return;
                }

                _log.Info("Автоматическое внесение времени.");

                var settings = ReadSettingsAndLock();

                await Task.Delay(2000);

                if (settings.DummyMode)
                {
                    settings.DummyMode = false;
                    _log.Info("Выключаем режим Тестового прогона");
                }

                using (var tokenSource = GetTokenSource())
                {
                    await _job.DoTheJob(settings, _tasksProcessors.WorkLogTasksProcessor, tokenSource.Token);
                }

                LockUnlock(true);
            }
        }

        private async void btnMeeting_Click(object sender, EventArgs e)
        {
            var settings = ReadSettingsAndLock();

            if (!Directory.Exists(settings.RepositoryPath))
            {
                MessageBox.Show("Папка с репо не сушествует.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (_tokenSource = GetTokenSource())
            {
                await _job.DoTheJob(settings, _tasksProcessors.MeetingProcessor, _tokenSource.Token);
            }

            LockUnlock(true);
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _tokenSource.Cancel();
        }
    }
}
