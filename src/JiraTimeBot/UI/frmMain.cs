using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using JiraTimeBot.Configuration;
using JiraTimeBot.DI;
using JiraTimeBot.TasksProcessors;
using JiraTimeBot.UI.Tray;
using JiraTimeBot.Update;

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

            _controls = new Control[] { txtDummyMode, btnStart, btnMeeting, btnSettings };

            _settingsWindowShow = () =>
            {
                var frmSettings = new frmSettings();
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
            this.Text = this.Text + " v." + Application.ProductVersion;

            PrintStartMessage();
            tmrStart.Enabled = true;

            if (System.Diagnostics.Debugger.IsAttached)
            {
                _log.Trace("Приложение запущено в режиме отладки. Отключаю обновление.");
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

            PrintStartMessage();
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrUpdate.Interval = 8 * 60 * 60 * 1000;

                var checker = new UpdateChecker();
                var release = checker.GetRelease();
                if (release == null)
                {
                    return;
                }

                var gitVersionStr = checker.GetLatestVersion(release);
                var gitVersion = new Version(gitVersionStr);
                var appVersion = new Version(Application.ProductVersion);

                if (gitVersion > appVersion)
                {
                    var downloadUrl = checker.GetDownloadUrl(release);
                    _log.Info($"Доступна новая версия: {gitVersion} {downloadUrl}");

                    var success = checker.DownloadFile(downloadUrl, "update.zip");
                    if (!success)
                    {
                        return;
                    }

                    var result = InstallUpdate(gitVersion, "update.zip");
                    if (result)
                    {
                        //если обновились - отключаем до рестарта
                        tmrUpdate.Enabled = false;
                    }
                }
                else
                {
                    _log.Info($"Вы используете последнюю версию.");
                }
            }
            catch (Exception ex)
            {
                //в процессе обновления ни при каком раскладе мы не должны уложить бота. 
                _log.Error($"Фатальная ошибка в процессе обновления: {ex.Message}");
            }
        }

        private bool InstallUpdate(Version gitVersion, string updateFile)
        {
            try
            {
                _log.Info($"Устанавливаю обновление до версии {gitVersion}.");
                //Rename he executing file
                System.IO.FileInfo appFile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var bkpDir = Path.Combine(appFile.DirectoryName, "bkp");
                if (!Directory.Exists(bkpDir))
                {
                    Directory.CreateDirectory(bkpDir);
                }

                using (ZipArchive archive = ZipFile.OpenRead(Path.Combine(appFile.DirectoryName, updateFile)))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        var zipEntryPath = Path.Combine(appFile.DirectoryName, entry.FullName);
                        if (File.Exists(zipEntryPath))
                        {
                            System.IO.FileInfo zipEntryFile = new System.IO.FileInfo(zipEntryPath);
                            if (File.Exists(bkpDir + "\\" + zipEntryFile.Name))
                            {
                                File.Delete(bkpDir + "\\" + zipEntryFile.Name);
                            }
                            System.IO.File.Move(zipEntryFile.FullName, bkpDir + "\\" + zipEntryFile.Name);
                        }

                        entry.ExtractToFile(zipEntryPath);
                    }
                }

                _log.Info("Обновление установлено. Перезапустите программу для применения изменений.");
                return true;
            }
            catch (Exception)
            {
                _log.Error($"Что-то пошло не так при обновлении.");
                return false;
            }
        }
    }
}
