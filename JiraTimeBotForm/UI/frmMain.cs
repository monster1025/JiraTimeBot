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
        private Settings _settings;

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

            _settings = Settings.Load();

            _job = _container.Resolve<Job>();
            _log = _container.Resolve<ILog>();
            _tasksProcessors = _container.Resolve<IAllTasksProcessors>();

            _controls = new Control[] { txtDummyMode, btnStart, btnMeeting, btnSettings };
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _trayIcon.Hide();
        }

        public Settings ReadSettingsAndLock()
        {
            _settings = Settings.Load();
            
            LockUnlock(false);

            return _settings;
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
        private void Form1_Load(object sender, EventArgs e)
        {
            _settings = Settings.Load();
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            var settings = ReadSettingsAndLock();

            using (_tokenSource = GetTokenSource())
            {
                await _job.DoTheJob(settings, _tasksProcessors.WorkLogTasksProcessor, txtDummyMode.Checked, _tokenSource.Token); 
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
            var runTime = _settings.ActivationTime;

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

                if (txtDummyMode.Checked)
                {
                    _log.Info("Выключаем режим Тестового прогона");
                }

                using (var tokenSource = GetTokenSource())
                {
                    await _job.DoTheJob(settings, _tasksProcessors.WorkLogTasksProcessor, txtDummyMode.Checked, tokenSource.Token);
                }

                LockUnlock(true);
            }
        }

        private async void btnMeeting_Click(object sender, EventArgs e)
        {
            var settings = ReadSettingsAndLock();

            using (_tokenSource = GetTokenSource())
            {
                await _job.DoTheJob(settings, _tasksProcessors.MeetingProcessor, txtDummyMode.Checked, _tokenSource.Token);
            }

            LockUnlock(true);
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _tokenSource.Cancel();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            var frm = new frmSettings();
            frm.ShowDialog();
        }
    }
}
