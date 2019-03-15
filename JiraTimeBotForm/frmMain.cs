using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using JiraTimeBotForm.CommitWorks;
using JiraTimeBotForm.Configuration;
using JiraTimeBotForm.Passwords;
using JiraTimeBotForm.TasksProcessors;
using Newtonsoft.Json;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace JiraTimeBotForm
{
    public partial class frmMain : Form
    {
        private readonly string _settingsPath;
        private readonly ILog _log;
        private readonly NotifyIcon  trayIcon;
        private readonly ContextMenu trayMenu;
        private Job _job;
        private readonly IReadOnlyList<Control> _controls;
        private CancellationTokenSource _tokenSource;
        private BuzzwordReplacer _buzzwordReplacer;

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
            _settingsPath = Path.Combine(Application.UserAppDataPath, "settings.json");
            _log = new Logger(txtLog);
            _buzzwordReplacer = new BuzzwordReplacer();

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);

            trayIcon = new NotifyIcon
            {
                Text = "JiraTimeBot", 
                Icon = this.Icon, 
                ContextMenu = trayMenu,
                Visible = false,

            };
            trayIcon.Click += btnTray_Click;
            trayIcon.DoubleClick += btnTray_Click;

            _job = new Job(_buzzwordReplacer, _log);

            _controls = new Control[] { txtJiraLogin, txtJiraPassword, txtMercurialEmail, actTime, txtRepoPath, txtDummyMode, btnSave, btnStart, btnMeeting, chkAddComments };
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            trayIcon.Visible = false;
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnTray_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.Activate();
            this.BringToFront();
            Extensions.Extensions.Restore(this);
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

            var password = new EncryptionClass().Encrypt(settings.JiraUserName, settings.JiraPassword, settings.JiraUrl);
            settings.JiraPassword = password;

            var settingsString = JsonConvert.SerializeObject(settings);
            File.WriteAllText(_settingsPath, settingsString);

            MessageBox.Show("Settings saved.");
            LoadSettings();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(_settingsPath))
            {
                LoadSettings();
            }
        }

        private void LoadSettings()
        {
            var settingsText = File.ReadAllText(_settingsPath);
            var settings = JsonConvert.DeserializeObject<Settings>(settingsText);

            var password = new EncryptionClass().Decrypt(settings.JiraUserName, settings.JiraPassword, settings.JiraUrl);

            txtJiraLogin.Text = settings.JiraUserName;
            txtJiraPassword.Text = password;
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
                await _job.DoTheJob(settings, new WorkLogTasksProcessor(_log), _tokenSource.Token); 
            }

            LockUnlock(true);
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                trayIcon.Visible = true;
                this.Hide();
            }

            else if (FormWindowState.Normal == this.WindowState)
            {
                trayIcon.Visible = false;
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
                    await _job.DoTheJob(settings, new WorkLogTasksProcessor(_log), tokenSource.Token);
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
                await _job.DoTheJob(settings, new MeetingProcessor(_log), _tokenSource.Token);
            }

            LockUnlock(true);
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _tokenSource.Cancel();
        }
    }
}
