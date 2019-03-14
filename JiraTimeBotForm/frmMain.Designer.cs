namespace JiraTimeBotForm
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.actTime = new System.Windows.Forms.DateTimePicker();
            this.txtJiraLogin = new System.Windows.Forms.TextBox();
            this.lbl1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtJiraPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMercurialEmail = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRepoPath = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.ntfyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.tmrStart = new System.Windows.Forms.Timer(this.components);
            this.txtDummyMode = new System.Windows.Forms.CheckBox();
            this.btnMeeting = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // actTime
            // 
            this.actTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.actTime.Location = new System.Drawing.Point(94, 120);
            this.actTime.Name = "actTime";
            this.actTime.Size = new System.Drawing.Size(100, 20);
            this.actTime.TabIndex = 0;
            // 
            // txtJiraLogin
            // 
            this.txtJiraLogin.Location = new System.Drawing.Point(94, 42);
            this.txtJiraLogin.Name = "txtJiraLogin";
            this.txtJiraLogin.Size = new System.Drawing.Size(100, 20);
            this.txtJiraLogin.TabIndex = 1;
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(28, 45);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(60, 13);
            this.lbl1.TabIndex = 2;
            this.lbl1.Text = "Логин Jira:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Пароль Jira:";
            // 
            // txtJiraPassword
            // 
            this.txtJiraPassword.Location = new System.Drawing.Point(94, 68);
            this.txtJiraPassword.Name = "txtJiraPassword";
            this.txtJiraPassword.PasswordChar = '*';
            this.txtJiraPassword.Size = new System.Drawing.Size(100, 20);
            this.txtJiraPassword.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Mercurial name:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtMercurialEmail
            // 
            this.txtMercurialEmail.Location = new System.Drawing.Point(94, 94);
            this.txtMercurialEmail.Name = "txtMercurialEmail";
            this.txtMercurialEmail.Size = new System.Drawing.Size(100, 20);
            this.txtMercurialEmail.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Время:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(62, 146);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Путь до репо:";
            // 
            // txtRepoPath
            // 
            this.txtRepoPath.Location = new System.Drawing.Point(94, 16);
            this.txtRepoPath.Name = "txtRepoPath";
            this.txtRepoPath.Size = new System.Drawing.Size(100, 20);
            this.txtRepoPath.TabIndex = 9;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(218, 16);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(510, 264);
            this.txtLog.TabIndex = 11;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(9, 224);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(114, 23);
            this.btnStart.TabIndex = 12;
            this.btnStart.Text = "Проставить время";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // ntfyIcon
            // 
            this.ntfyIcon.Text = "notifyIcon1";
            this.ntfyIcon.Visible = true;
            // 
            // tmrStart
            // 
            this.tmrStart.Enabled = true;
            this.tmrStart.Interval = 1500;
            this.tmrStart.Tick += new System.EventHandler(this.tmrStart_Tick);
            // 
            // txtDummyMode
            // 
            this.txtDummyMode.AutoSize = true;
            this.txtDummyMode.Location = new System.Drawing.Point(9, 201);
            this.txtDummyMode.Name = "txtDummyMode";
            this.txtDummyMode.Size = new System.Drawing.Size(114, 17);
            this.txtDummyMode.TabIndex = 13;
            this.txtDummyMode.Text = "Тестовый прогон";
            this.txtDummyMode.UseVisualStyleBackColor = true;
            // 
            // btnMeeting
            // 
            this.btnMeeting.Location = new System.Drawing.Point(142, 224);
            this.btnMeeting.Name = "btnMeeting";
            this.btnMeeting.Size = new System.Drawing.Size(70, 23);
            this.btnMeeting.TabIndex = 14;
            this.btnMeeting.Text = "Митинг";
            this.btnMeeting.UseVisualStyleBackColor = true;
            this.btnMeeting.Click += new System.EventHandler(this.btnMeeting_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(9, 256);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(203, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel operation";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 292);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnMeeting);
            this.Controls.Add(this.txtDummyMode);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtRepoPath);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMercurialEmail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtJiraPassword);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.txtJiraLogin);
            this.Controls.Add(this.actTime);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmMain";
            this.Text = "Бот логирования времени";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker actTime;
        private System.Windows.Forms.TextBox txtJiraLogin;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtJiraPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMercurialEmail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRepoPath;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.NotifyIcon ntfyIcon;
        private System.Windows.Forms.Timer tmrStart;
        private System.Windows.Forms.CheckBox txtDummyMode;
        private System.Windows.Forms.Button btnMeeting;
        private System.Windows.Forms.Button btnCancel;
    }
}

