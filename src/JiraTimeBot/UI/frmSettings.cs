﻿using JiraTimeBot.Configuration;
using JiraTimeBot.UI.Startup;
using System;
using System.Windows.Forms;

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
            txtJiraUrl.Text = settings.JiraUrl;
            txtJiraLogin.Text = settings.JiraUserName;
            txtJiraPassword.Text = settings.JiraPassword;
            txtMercurialEmail.Text = settings.MercurialAuthorEmail;
            txtRepoPath.Text = settings.RepositoryPath;
            actTime.Text = settings.ActivationTime.ToString("hh\\:mm\\:ss");
            chkAddComments.Checked = settings.AddCommentsToWorklog;
            txtRoundTo.Text = settings.RoundToMinutes.ToString();
            cboWorkType.SelectedIndex = (int)settings.WorkType;
            txtTimeControlTask.Text = settings.TimeControlTask;
            txtJQL.Text = settings.JiraQuery;
            txtWorkDayDuration.Text = settings.MinuterPerWorkDay.ToString();
            txtRandomMinutes.Text = settings.RandomWorkMinutes.ToString();
            chkPullBeforeProcess.Checked = settings.PullBeforeProcess;
            chkRemoveManuallyAddedWorklogs.Checked = settings.RemoveManuallyAddedWorklogs;
            tbAlternateEmail.Text = settings.AlternativeEmail;
            cboWorkType_SelectedIndexChanged(null, null);
        }

        public Settings ReadSettingsAndLock()
        {
            if (!int.TryParse(txtRoundTo.Text, out var roundTo))
            {
                roundTo = 15;
            }
            if (!int.TryParse(txtWorkDayDuration.Text, out var minuterPerWorkDay))
            {
                minuterPerWorkDay = 8 * 60;
            }
            if (!int.TryParse(txtRandomMinutes.Text, out var randomWorkMinutes))
            {
                randomWorkMinutes = 0;
            }

            var settings = new Settings
            {
                JiraUrl = txtJiraUrl.Text,
                JiraUserName = txtJiraLogin.Text,
                JiraPassword = txtJiraPassword.Text,
                MercurialAuthorEmail = txtMercurialEmail.Text,
                ActivationTime = TimeSpan.Parse(actTime.Text),
                RepositoryPath = txtRepoPath.Text,
                AddCommentsToWorklog = chkAddComments.Checked,
                RoundToMinutes = roundTo,
                WorkType = (WorkType)cboWorkType.SelectedIndex,
                JiraQuery = txtJQL.Text,
                TimeControlTask = txtTimeControlTask.Text,
                MinuterPerWorkDay = minuterPerWorkDay,
                RandomWorkMinutes = randomWorkMinutes,
                PullBeforeProcess = chkPullBeforeProcess.Checked,
                RemoveManuallyAddedWorklogs = chkRemoveManuallyAddedWorklogs.Checked,
                AlternativeEmail = tbAlternateEmail.Text
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
            txtRepoPath.Enabled = (cboWorkType.SelectedIndex == 0 
                                   || cboWorkType.SelectedIndex == 2
                                   || cboWorkType.SelectedIndex == 3);
        }
    }
}
