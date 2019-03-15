using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JiraTimeBotForm.Configuration;

namespace JiraTimeBotForm.UI
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            var settings = Settings.Load();
            SetSettings(settings);
        }

        private void SetSettings(Settings settings)
        {
            txtJiraLogin.Text = settings.JiraUserName;
            txtJiraPassword.Text = settings.JiraPassword;
            txtMercurialEmail.Text = settings.MercurialAuthorEmail;
            txtRepoPath.Text = settings.RepositoryPath;
            actTime.Text = settings.ActivationTime.ToString("hh\\:mm\\:ss");
            //txtDummyMode.Checked = settings.DummyMode;
            chkAddComments.Checked = settings.AddCommentsToWorklog;
            txtRoundTo.Text = settings.RountToMinutes.ToString();
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
                //DummyMode = txtDummyMode.Checked,
                AddCommentsToWorklog = chkAddComments.Checked,
                RountToMinutes = roundTo
            };
            
            //LockUnlock(false);

            return settings;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var settings = ReadSettingsAndLock();
            settings.Save();
            MessageBox.Show("Настройки сохранены.");
            this.Close();
        }
    }
}
