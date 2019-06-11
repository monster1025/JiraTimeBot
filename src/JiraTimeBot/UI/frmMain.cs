using Autofac;
using JiraTimeBot.Configuration;
using JiraTimeBot.DI;
using JiraTimeBot.TasksProcessors;
using JiraTimeBot.UI.Tray;
using JiraTimeBot.Update;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using JiraTimeBot.UI.Startup;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace JiraTimeBot.UI
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
        private readonly Action _settingsWindowShow;
        private readonly Action<string> _settingsErrorReporter;
        private bool _updateChecked;

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE) {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                _trayIcon.Hide();
            }
            base.WndProc(ref message);
        }

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

            _controls = new Control[] { txtDummyMode, btnStart, btnMeeting, btnSettings, dteForDay, btnDoForDate };

            _settingsWindowShow = () =>
            {
                var frmSettings = new frmSettings(_container.Resolve<AutoStartUp>());
                frmSettings.ShowDialog(this);
            };
            _settingsErrorReporter = msg => MessageBox.Show(msg, "Загрузка настроек", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _trayIcon.Hide();
        }

        public Settings ReadSettingsAndLock()
        {
            _settings = Settings.LoadAndCheck(_settingsWindowShow, _settingsErrorReporter);
            
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
            this.Text = this.Text + " v" + Application.ProductVersion;

            PrintStartMessage();
            tmrStart.Enabled = true;

            if (System.Diagnostics.Debugger.IsAttached)
            {
                _log.Trace("Приложение запущено в режиме отладки. Отключаю обновление.");
                //tmrUpdate.Enabled = true;
            }
            else
            {
                tmrUpdate.Enabled = true;
            }
        }

        private void PrintStartMessage()
        {
            _settings = Settings.LoadAndCheck(_settingsWindowShow, _settingsErrorReporter);
            _log.Info($"Загружен бот для {_settings.JiraUserName}, Режим: {_settings.WorkType.ToString()}, работаем в {_settings.RepositoryPath}");
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
                    txtDummyMode.Checked = false;
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
            var frm = _container.Resolve<frmSettings>();
            frm.ShowDialog();

            PrintStartMessage();
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            try
            {
                var firstTime = !_updateChecked;
                _updateChecked = true;
                tmrUpdate.Interval = 1 * 60 * 60 * 1000;

                var updater = _container.Resolve<IUpdater>();
                if (updater.UpdateToNewVersion(firstTime))
                {
                    //если обновились - отключаем до рестарта
                    tmrUpdate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //в процессе обновления ни при каком раскладе мы не должны уложить бота. 
                _log.Error($"Фатальная ошибка в процессе обновления: {ex.Message}");
            }
        }

        private async void btnDoForDate_Click(object sender, EventArgs e)
        {
            var settings = ReadSettingsAndLock();

            using (_tokenSource = GetTokenSource())
            {
                await _job.SetTaskTimesForDate(dteForDay.Value, dteForDay.Value, settings, _tasksProcessors.WorkLogTasksProcessor, txtDummyMode.Checked, _tokenSource.Token);
            }

            LockUnlock(true);

        }
    }
}
