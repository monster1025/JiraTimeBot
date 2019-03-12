using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using JiraTimeBotForm.Configuration;
using JiraTimeBotForm.JiraIntegration;
using JiraTimeBotForm.Passwords;
using JiraTimeBotForm.TaskTime;
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
 
        public frmMain()
        {
            InitializeComponent();
            _settingsPath = Path.Combine(Application.UserAppDataPath, "settings.json");
            _log = new Logger(txtLog);

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);
 
            trayIcon      = new NotifyIcon();
            trayIcon.Text = "JiraTimeBot";
            trayIcon.Icon = this.Icon;// new Icon(SystemIcons.WinLogo, 40, 40);

            trayIcon.ContextMenu = trayMenu;
            trayIcon.Click += btnTray_Click;
            trayIcon.DoubleClick += btnTray_Click;
            trayIcon.Visible = false;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            var settings = new Settings
            {
                JiraUserName = txtJiraLogin.Text,
                JiraPassword = txtJiraPassword.Text,
                MercurialAuthorEmail = txtMercurialEmail.Text,
                ActivationTime = TimeSpan.Parse(actTime.Text),
                RepositoryPath = txtRepoPath.Text

            };

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
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            DoTheJob();
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

        private void tmrStart_Tick(object sender, EventArgs e)
        {
            var runTime = TimeSpan.Parse(actTime.Text);
            if (Math.Abs((DateTime.Now.TimeOfDay - runTime).TotalSeconds) < 1)
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    _log.Error("сегодня суббота или воскресенье. Работать нельзя =)");
                    return;
                }

                txtDummyMode.Checked = false;
                DoTheJob();
            }
        }

        private void DoTheJob()
        {
            if (!Directory.Exists(txtRepoPath.Text))
            {
                MessageBox.Show("Папка с репо не сушествует.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtLog.Text = "";
            var settings = new Settings
            {
                JiraUserName = txtJiraLogin.Text,
                JiraPassword = txtJiraPassword.Text,
                MercurialAuthorEmail = txtMercurialEmail.Text,
                ActivationTime = TimeSpan.Parse(actTime.Text),
                RepositoryPath = txtRepoPath.Text
            };

            var jira = new JiraApi(settings, _log);
            var taskDiscoverer = new TaskTimeDiscoverer(_log);

            int daysDiff = 0;
            while (true)
            {
                var date = DateTime.Now.Date.AddDays(daysDiff);
                var taskTimes = taskDiscoverer.GetTaskTimes(settings, date);
                if (!taskTimes.Any())
                {
                    _log.Warn($"{date:dd.MM.yyyy} вы не сделали ничего полезного =) Использую предыдущий день.");
                    daysDiff--;
                    if (daysDiff < -7)
                    {
                        _log.Error("Не нашли ни одного коммита за предыдущие 7 дней. Возможно вы в отпуске? Выхожу.");
                        return;
                    }

                    continue;
                }

                _log.Trace($"На реальную дату {date:dd.MM.yyyy} распределение по задачам:");
                foreach (var taskTime in taskTimes)
                {
                    _log.Trace($"- {taskTime.Branch} (коммитов {taskTime.Commits}): {taskTime.Time}");
                }

                jira.SetTodayWorklog(taskTimes, dummy: txtDummyMode.Checked);
                _log.Info("Готово.");
                return;
            }
        }

        private void btnMeeting_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtRepoPath.Text))
            {
                MessageBox.Show("Папка с репо не сушествует.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtLog.Text = "";
            var settings = new Settings
            {
                JiraUserName = txtJiraLogin.Text,
                JiraPassword = txtJiraPassword.Text,
                MercurialAuthorEmail = txtMercurialEmail.Text,
                ActivationTime = TimeSpan.Parse(actTime.Text),
                RepositoryPath = txtRepoPath.Text
            };

            var jira = new JiraApi(settings, _log);
            var taskDiscoverer = new TaskTimeDiscoverer();

            int daysDiff = -1;
            while (true)
            {
                var date = DateTime.Now.Date.AddDays(daysDiff);
                var taskTimes = taskDiscoverer.GetTaskTimes(settings, date);
                if (!taskTimes.Any())
                {
                    _log.Warn($"{date:dd.MM.yyyy} вы не сделали ничего полезного =) Использую предыдущий день.");
                    daysDiff--;
                    if (daysDiff < -7)
                    {
                        _log.Error("Не нашли ни одного коммита за предыдущие 7 дней. Возможно вы в отпуске? Выхожу.");
                        return;
                    }

                    continue;
                }

                _log.Trace($"На реальную дату {date:dd.MM.yyyy} распределение по задачам:");
                foreach (var taskTime in taskTimes.OrderByDescending(f=>f.Time))
                {
                    var taskName = jira.GetTaskName(taskTime.Branch);
                    _log.Trace($"- {taskTime.Branch}: {taskName} - {taskTime.Time}");
                }

                break;
            }

        }
    }
}
