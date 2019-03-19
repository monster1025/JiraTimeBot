namespace JiraTimeBot.UI
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
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.ntfyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.tmrStart = new System.Windows.Forms.Timer(this.components);
            this.txtDummyMode = new System.Windows.Forms.CheckBox();
            this.btnMeeting = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(132, 16);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(596, 294);
            this.txtLog.TabIndex = 11;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 229);
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
            this.tmrStart.Interval = 1500;
            this.tmrStart.Tick += new System.EventHandler(this.tmrStart_Tick);
            // 
            // txtDummyMode
            // 
            this.txtDummyMode.AutoSize = true;
            this.txtDummyMode.Location = new System.Drawing.Point(12, 206);
            this.txtDummyMode.Name = "txtDummyMode";
            this.txtDummyMode.Size = new System.Drawing.Size(114, 17);
            this.txtDummyMode.TabIndex = 13;
            this.txtDummyMode.Text = "Тестовый прогон";
            this.txtDummyMode.UseVisualStyleBackColor = true;
            // 
            // btnMeeting
            // 
            this.btnMeeting.Location = new System.Drawing.Point(12, 258);
            this.btnMeeting.Name = "btnMeeting";
            this.btnMeeting.Size = new System.Drawing.Size(114, 23);
            this.btnMeeting.TabIndex = 14;
            this.btnMeeting.Text = "Митинг";
            this.btnMeeting.UseVisualStyleBackColor = true;
            this.btnMeeting.Click += new System.EventHandler(this.btnMeeting_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(12, 287);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(114, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Отменить операцию";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(12, 16);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(114, 23);
            this.btnSettings.TabIndex = 16;
            this.btnSettings.Text = "Настройки";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 1000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 320);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnMeeting);
            this.Controls.Add(this.txtDummyMode);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtLog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Бот логирования времени";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.NotifyIcon ntfyIcon;
        private System.Windows.Forms.Timer tmrStart;
        private System.Windows.Forms.CheckBox txtDummyMode;
        private System.Windows.Forms.Button btnMeeting;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Timer tmrUpdate;
    }
}

