using System;
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
            //txtDummyMode.Checked = settings.DummyMode;
            chkAddComments.Checked = settings.AddCommentsToWorklog;
            txtRoundTo.Text = settings.RountToMinutes.ToString();
            cboWorkType.SelectedIndex = (int) settings.WorkType;
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
                RountToMinutes = roundTo,
                WorkType = (WorkType) cboWorkType.SelectedIndex
            };
            
            //LockUnlock(false);

            return settings;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var settings = ReadSettingsAndLock();
            settings.Save();

            MessageBox.Show("Настройки сохранены.");

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void txtRoundTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
