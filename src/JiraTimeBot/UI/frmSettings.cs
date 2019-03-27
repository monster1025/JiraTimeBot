using System;
using System.Windows.Forms;
using JiraTimeBot.Configuration;
using JiraTimeBot.UI.Startup;

namespace JiraTimeBot.UI
{
    public partial class frmSettings : Form
    {
        private readonly AutoStartUp _autoStart;

        public frmSettings(AutoStartUp autoStart)
        {
            _autoStart = autoStart;
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            var settings = Settings.Load();
            if (settings != null)
            {
                SetSettings(settings);
            }

            chkAutostart.Checked = _autoStart.Get();
        }

        private void SetSettings(Settings settings)
        {
            txtJiraLogin.Text = settings.JiraUserName;
            txtJiraPassword.Text = settings.JiraPassword;
            txtMercurialEmail.Text = settings.MercurialAuthorEmail;
            txtRepoPath.Text = settings.RepositoryPath;
            actTime.Text = settings.ActivationTime.ToString("hh\\:mm\\:ss");
            chkAddComments.Checked = settings.AddCommentsToWorklog;
            txtRoundTo.Text = settings.RountToMinutes.ToString();
            cboWorkType.SelectedIndex = (int) settings.WorkType;
            txtTimeControlTask.Text = settings.TimeControlTask;
            chkRemoveAddedByUser.Checked = settings.RemoveWorklogsAddedByUser;
            txtJQL.Text = settings.JiraQuery;
            txtJQL.Enabled = settings.WorkType == WorkType.JiraLogs;
        }

        public Settings ReadSettingsAndLock()
        {
            if (!int.TryParse(txtRoundTo.Text, out var roundTo))
            {
                roundTo = 15;
            }

            var settings = new Settings
            {
                JiraUserName = txtJiraLogin.Text,
                JiraPassword = txtJiraPassword.Text,
                MercurialAuthorEmail = txtMercurialEmail.Text,
                ActivationTime = TimeSpan.Parse(actTime.Text),
                RepositoryPath = txtRepoPath.Text,
                AddCommentsToWorklog = chkAddComments.Checked,
                RountToMinutes = roundTo,
                WorkType = (WorkType) cboWorkType.SelectedIndex,
                JiraQuery = txtJQL.Text,
                TimeControlTask = txtTimeControlTask.Text,
                RemoveWorklogsAddedByUser = chkRemoveAddedByUser.Checked
            };

            //LockUnlock(false);

            return settings;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var settings = ReadSettingsAndLock();
            settings.Save();
             _autoStart.Set(chkAutostart.Checked);

            MessageBox.Show("Настройки сохранены.");

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void txtRoundTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cboWorkType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtJQL.Enabled = (cboWorkType.SelectedIndex == 1);
        }
    }
}
