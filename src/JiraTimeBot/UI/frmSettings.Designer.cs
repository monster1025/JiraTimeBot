namespace JiraTimeBot.UI
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label5 = new System.Windows.Forms.Label();
            this.txtRoundTo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMercurialEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtJiraPassword = new System.Windows.Forms.TextBox();
            this.lbl1 = new System.Windows.Forms.Label();
            this.txtJiraLogin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.actTime = new System.Windows.Forms.DateTimePicker();
            this.chkAddComments = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.cboWorkType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtJQL = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTimeControlTask = new System.Windows.Forms.TextBox();
            this.chkAutostart = new System.Windows.Forms.CheckBox();
            this.lblWorkDay = new System.Windows.Forms.Label();
            this.txtWorkDayDuration = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRandomMinutes = new System.Windows.Forms.TextBox();
            this.chkPullBeforeProcess = new System.Windows.Forms.CheckBox();
            this.txtRepoPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblJiraUrl = new System.Windows.Forms.Label();
            this.txtJiraUrl = new System.Windows.Forms.TextBox();
            this.chkRemoveManuallyAddedWorklogs = new System.Windows.Forms.CheckBox();
            this.tbAlternateEmail = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Right;
            this.label5.Location = new System.Drawing.Point(36, 185);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 26);
            this.label5.TabIndex = 28;
            this.label5.Text = "Округл. времени*:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRoundTo
            // 
            this.txtRoundTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRoundTo.Location = new System.Drawing.Point(142, 188);
            this.txtRoundTo.Name = "txtRoundTo";
            this.txtRoundTo.Size = new System.Drawing.Size(180, 20);
            this.txtRoundTo.TabIndex = 27;
            this.txtRoundTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRoundTo_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Location = new System.Drawing.Point(54, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 26);
            this.label2.TabIndex = 24;
            this.label2.Text = "Mercurial name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMercurialEmail
            // 
            this.txtMercurialEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMercurialEmail.Location = new System.Drawing.Point(142, 162);
            this.txtMercurialEmail.Name = "txtMercurialEmail";
            this.txtMercurialEmail.Size = new System.Drawing.Size(180, 20);
            this.txtMercurialEmail.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Location = new System.Drawing.Point(65, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 26);
            this.label1.TabIndex = 22;
            this.label1.Text = "Пароль Jira*:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtJiraPassword
            // 
            this.txtJiraPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJiraPassword.Location = new System.Drawing.Point(142, 136);
            this.txtJiraPassword.Name = "txtJiraPassword";
            this.txtJiraPassword.PasswordChar = '*';
            this.txtJiraPassword.Size = new System.Drawing.Size(180, 20);
            this.txtJiraPassword.TabIndex = 21;
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbl1.Location = new System.Drawing.Point(72, 107);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(64, 26);
            this.lbl1.TabIndex = 20;
            this.lbl1.Text = "Логин Jira*:";
            this.lbl1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtJiraLogin
            // 
            this.txtJiraLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJiraLogin.Location = new System.Drawing.Point(142, 110);
            this.txtJiraLogin.Name = "txtJiraLogin";
            this.txtJiraLogin.Size = new System.Drawing.Size(180, 20);
            this.txtJiraLogin.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Location = new System.Drawing.Point(45, 263);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 26);
            this.label3.TabIndex = 30;
            this.label3.Text = "Время запуска*:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // actTime
            // 
            this.actTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.actTime.Location = new System.Drawing.Point(142, 266);
            this.actTime.Name = "actTime";
            this.actTime.Size = new System.Drawing.Size(100, 20);
            this.actTime.TabIndex = 29;
            // 
            // chkAddComments
            // 
            this.chkAddComments.AutoSize = true;
            this.chkAddComments.Location = new System.Drawing.Point(328, 30);
            this.chkAddComments.Name = "chkAddComments";
            this.chkAddComments.Size = new System.Drawing.Size(133, 17);
            this.chkAddComments.TabIndex = 31;
            this.chkAddComments.Text = "Добавлять описание";
            this.chkAddComments.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(328, 344);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 32;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cboWorkType
            // 
            this.cboWorkType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWorkType.FormattingEnabled = true;
            this.cboWorkType.Items.AddRange(new object[] {
            "Mercurial",
            "Jira",
            "Git",
            "CvsMixed"});
            this.cboWorkType.Location = new System.Drawing.Point(142, 3);
            this.cboWorkType.Name = "cboWorkType";
            this.cboWorkType.Size = new System.Drawing.Size(180, 21);
            this.cboWorkType.TabIndex = 34;
            this.cboWorkType.SelectedIndexChanged += new System.EventHandler(this.cboWorkType_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "Режим работы:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Right;
            this.label7.Location = new System.Drawing.Point(107, 211);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 26);
            this.label7.TabIndex = 37;
            this.label7.Text = "JQL:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtJQL
            // 
            this.txtJQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJQL.Location = new System.Drawing.Point(142, 214);
            this.txtJQL.Name = "txtJQL";
            this.txtJQL.Size = new System.Drawing.Size(180, 20);
            this.txtJQL.TabIndex = 36;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Right;
            this.label8.Location = new System.Drawing.Point(3, 237);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(133, 26);
            this.label8.TabIndex = 39;
            this.label8.Text = "Задача \"учёта времени\":";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTimeControlTask
            // 
            this.txtTimeControlTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTimeControlTask.Location = new System.Drawing.Point(142, 240);
            this.txtTimeControlTask.Name = "txtTimeControlTask";
            this.txtTimeControlTask.Size = new System.Drawing.Size(180, 20);
            this.txtTimeControlTask.TabIndex = 38;
            // 
            // chkAutostart
            // 
            this.chkAutostart.AutoSize = true;
            this.chkAutostart.Location = new System.Drawing.Point(328, 3);
            this.chkAutostart.Name = "chkAutostart";
            this.chkAutostart.Size = new System.Drawing.Size(96, 17);
            this.chkAutostart.TabIndex = 40;
            this.chkAutostart.Text = "Автозагрузка";
            this.chkAutostart.UseVisualStyleBackColor = true;
            // 
            // lblWorkDay
            // 
            this.lblWorkDay.AutoSize = true;
            this.lblWorkDay.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblWorkDay.Location = new System.Drawing.Point(19, 289);
            this.lblWorkDay.Name = "lblWorkDay";
            this.lblWorkDay.Size = new System.Drawing.Size(117, 26);
            this.lblWorkDay.TabIndex = 42;
            this.lblWorkDay.Text = "Минут в рабочем дне:";
            this.lblWorkDay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtWorkDayDuration
            // 
            this.txtWorkDayDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWorkDayDuration.Location = new System.Drawing.Point(142, 292);
            this.txtWorkDayDuration.Name = "txtWorkDayDuration";
            this.txtWorkDayDuration.Size = new System.Drawing.Size(180, 20);
            this.txtWorkDayDuration.TabIndex = 41;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Right;
            this.label9.Location = new System.Drawing.Point(35, 315);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 26);
            this.label9.TabIndex = 44;
            this.label9.Text = "Рандомных минут:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRandomMinutes
            // 
            this.txtRandomMinutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRandomMinutes.Location = new System.Drawing.Point(142, 318);
            this.txtRandomMinutes.Name = "txtRandomMinutes";
            this.txtRandomMinutes.Size = new System.Drawing.Size(180, 20);
            this.txtRandomMinutes.TabIndex = 43;
            // 
            // chkPullBeforeProcess
            // 
            this.chkPullBeforeProcess.AutoSize = true;
            this.chkPullBeforeProcess.Location = new System.Drawing.Point(328, 110);
            this.chkPullBeforeProcess.Name = "chkPullBeforeProcess";
            this.chkPullBeforeProcess.Size = new System.Drawing.Size(83, 17);
            this.chkPullBeforeProcess.TabIndex = 45;
            this.chkPullBeforeProcess.Text = "Делать pull";
            this.chkPullBeforeProcess.UseVisualStyleBackColor = true;
            // 
            // txtRepoPath
            // 
            this.txtRepoPath.Location = new System.Drawing.Point(142, 30);
            this.txtRepoPath.Multiline = true;
            this.txtRepoPath.Name = "txtRepoPath";
            this.txtRepoPath.Size = new System.Drawing.Size(180, 48);
            this.txtRepoPath.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 54);
            this.label4.TabIndex = 26;
            this.label4.Text = "Путь до репо:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblJiraUrl
            // 
            this.lblJiraUrl.AutoSize = true;
            this.lblJiraUrl.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblJiraUrl.Location = new System.Drawing.Point(85, 81);
            this.lblJiraUrl.Name = "lblJiraUrl";
            this.lblJiraUrl.Size = new System.Drawing.Size(51, 26);
            this.lblJiraUrl.TabIndex = 47;
            this.lblJiraUrl.Text = "Jira URL:";
            this.lblJiraUrl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtJiraUrl
            // 
            this.txtJiraUrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJiraUrl.Location = new System.Drawing.Point(142, 84);
            this.txtJiraUrl.Name = "txtJiraUrl";
            this.txtJiraUrl.Size = new System.Drawing.Size(180, 20);
            this.txtJiraUrl.TabIndex = 46;
            // 
            // chkRemoveManuallyAddedWorklogs
            // 
            this.chkRemoveManuallyAddedWorklogs.AutoSize = true;
            this.chkRemoveManuallyAddedWorklogs.Location = new System.Drawing.Point(328, 84);
            this.chkRemoveManuallyAddedWorklogs.Name = "chkRemoveManuallyAddedWorklogs";
            this.chkRemoveManuallyAddedWorklogs.Size = new System.Drawing.Size(155, 17);
            this.chkRemoveManuallyAddedWorklogs.TabIndex = 48;
            this.chkRemoveManuallyAddedWorklogs.Text = "Удалять \"ручные записи\"";
            this.chkRemoveManuallyAddedWorklogs.UseVisualStyleBackColor = true;
            // 
            // tbAlternateEmail
            // 
            this.tbAlternateEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbAlternateEmail.Location = new System.Drawing.Point(142, 344);
            this.tbAlternateEmail.Name = "tbAlternateEmail";
            this.tbAlternateEmail.Size = new System.Drawing.Size(180, 20);
            this.tbAlternateEmail.TabIndex = 49;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Right;
            this.label10.Location = new System.Drawing.Point(13, 341);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(123, 29);
            this.label10.TabIndex = 50;
            this.label10.Text = "Дополнительный Email";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.chkPullBeforeProcess, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.chkRemoveManuallyAddedWorklogs, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkAddComments, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.cboWorkType, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkAutostart, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbAlternateEmail, 1, 12);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.lblJiraUrl, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtRandomMinutes, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.txtRepoPath, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblWorkDay, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.txtJiraUrl, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtWorkDayDuration, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.actTime, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.txtJiraLogin, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtTimeControlTask, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtJiraPassword, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtJQL, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtMercurialEmail, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtRoundTo, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.lbl1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 2, 12);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 13;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(486, 370);
            this.tableLayoutPanel1.TabIndex = 51;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 372);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRoundTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMercurialEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtJiraPassword;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.TextBox txtJiraLogin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker actTime;
        private System.Windows.Forms.CheckBox chkAddComments;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cboWorkType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtJQL;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtTimeControlTask;
        private System.Windows.Forms.CheckBox chkAutostart;
        private System.Windows.Forms.Label lblWorkDay;
        private System.Windows.Forms.TextBox txtWorkDayDuration;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtRandomMinutes;
        private System.Windows.Forms.CheckBox chkPullBeforeProcess;
        private System.Windows.Forms.TextBox txtRepoPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblJiraUrl;
        private System.Windows.Forms.TextBox txtJiraUrl;
        private System.Windows.Forms.CheckBox chkRemoveManuallyAddedWorklogs;
        private System.Windows.Forms.TextBox tbAlternateEmail;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}