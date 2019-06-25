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
            this.label4 = new System.Windows.Forms.Label();
            this.txtRepoPath = new System.Windows.Forms.TextBox();
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
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Округл. времени*:";
            // 
            // txtRoundTo
            // 
            this.txtRoundTo.Location = new System.Drawing.Point(133, 150);
            this.txtRoundTo.Name = "txtRoundTo";
            this.txtRoundTo.Size = new System.Drawing.Size(100, 20);
            this.txtRoundTo.TabIndex = 27;
            this.txtRoundTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRoundTo_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(53, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Путь до репо:";
            // 
            // txtRepoPath
            // 
            this.txtRepoPath.Location = new System.Drawing.Point(133, 45);
            this.txtRepoPath.Name = "txtRepoPath";
            this.txtRepoPath.Size = new System.Drawing.Size(100, 20);
            this.txtRepoPath.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Mercurial name:";
            // 
            // txtMercurialEmail
            // 
            this.txtMercurialEmail.Location = new System.Drawing.Point(133, 124);
            this.txtMercurialEmail.Name = "txtMercurialEmail";
            this.txtMercurialEmail.Size = new System.Drawing.Size(100, 20);
            this.txtMercurialEmail.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Пароль Jira*:";
            // 
            // txtJiraPassword
            // 
            this.txtJiraPassword.Location = new System.Drawing.Point(133, 97);
            this.txtJiraPassword.Name = "txtJiraPassword";
            this.txtJiraPassword.PasswordChar = '*';
            this.txtJiraPassword.Size = new System.Drawing.Size(100, 20);
            this.txtJiraPassword.TabIndex = 21;
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(69, 74);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(64, 13);
            this.lbl1.TabIndex = 20;
            this.lbl1.Text = "Логин Jira*:";
            // 
            // txtJiraLogin
            // 
            this.txtJiraLogin.Location = new System.Drawing.Point(133, 71);
            this.txtJiraLogin.Name = "txtJiraLogin";
            this.txtJiraLogin.Size = new System.Drawing.Size(100, 20);
            this.txtJiraLogin.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 232);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Время запуска*:";
            // 
            // actTime
            // 
            this.actTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.actTime.Location = new System.Drawing.Point(133, 228);
            this.actTime.Name = "actTime";
            this.actTime.Size = new System.Drawing.Size(100, 20);
            this.actTime.TabIndex = 29;
            // 
            // chkAddComments
            // 
            this.chkAddComments.AutoSize = true;
            this.chkAddComments.Location = new System.Drawing.Point(65, 308);
            this.chkAddComments.Name = "chkAddComments";
            this.chkAddComments.Size = new System.Drawing.Size(133, 17);
            this.chkAddComments.TabIndex = 31;
            this.chkAddComments.Text = "Добавлять описание";
            this.chkAddComments.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(86, 331);
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
            "Jira"});
            this.cboWorkType.Location = new System.Drawing.Point(133, 18);
            this.cboWorkType.Name = "cboWorkType";
            this.cboWorkType.Size = new System.Drawing.Size(99, 21);
            this.cboWorkType.TabIndex = 34;
            this.cboWorkType.SelectedIndexChanged += new System.EventHandler(this.cboWorkType_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(44, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "Режим работы:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(100, 179);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 37;
            this.label7.Text = "JQL:";
            // 
            // txtJQL
            // 
            this.txtJQL.Location = new System.Drawing.Point(133, 176);
            this.txtJQL.Name = "txtJQL";
            this.txtJQL.Size = new System.Drawing.Size(99, 20);
            this.txtJQL.TabIndex = 36;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(-2, 206);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(133, 13);
            this.label8.TabIndex = 39;
            this.label8.Text = "Задача \"учёта времени\":";
            // 
            // txtTimeControlTask
            // 
            this.txtTimeControlTask.Location = new System.Drawing.Point(133, 202);
            this.txtTimeControlTask.Name = "txtTimeControlTask";
            this.txtTimeControlTask.Size = new System.Drawing.Size(99, 20);
            this.txtTimeControlTask.TabIndex = 38;
            // 
            // chkAutostart
            // 
            this.chkAutostart.AutoSize = true;
            this.chkAutostart.Location = new System.Drawing.Point(65, 285);
            this.chkAutostart.Name = "chkAutostart";
            this.chkAutostart.Size = new System.Drawing.Size(96, 17);
            this.chkAutostart.TabIndex = 40;
            this.chkAutostart.Text = "Автозагрузка";
            this.chkAutostart.UseVisualStyleBackColor = true;
            // 
            // lblWorkDay
            // 
            this.lblWorkDay.AutoSize = true;
            this.lblWorkDay.Location = new System.Drawing.Point(15, 258);
            this.lblWorkDay.Name = "lblWorkDay";
            this.lblWorkDay.Size = new System.Drawing.Size(117, 13);
            this.lblWorkDay.TabIndex = 42;
            this.lblWorkDay.Text = "Минут в рабочем дне:";
            // 
            // txtWorkDayDuration
            // 
            this.txtWorkDayDuration.Location = new System.Drawing.Point(133, 254);
            this.txtWorkDayDuration.Name = "txtWorkDayDuration";
            this.txtWorkDayDuration.Size = new System.Drawing.Size(99, 20);
            this.txtWorkDayDuration.TabIndex = 41;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 362);
            this.Controls.Add(this.lblWorkDay);
            this.Controls.Add(this.txtWorkDayDuration);
            this.Controls.Add(this.chkAutostart);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtTimeControlTask);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtJQL);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboWorkType);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkAddComments);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.actTime);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtRoundTo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtRepoPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMercurialEmail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtJiraPassword);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.txtJiraLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRoundTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRepoPath;
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
    }
}