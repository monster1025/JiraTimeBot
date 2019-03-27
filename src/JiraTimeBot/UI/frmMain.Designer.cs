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
            this.ntfyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.tmrStart = new System.Windows.Forms.Timer(this.components);
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.tlpControls = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnMeeting = new System.Windows.Forms.Button();
            this.txtDummyMode = new System.Windows.Forms.CheckBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.pnlTopControls = new System.Windows.Forms.Panel();
            this.btnDoForDate = new System.Windows.Forms.Button();
            this.dteForDay = new System.Windows.Forms.DateTimePicker();
            this.btnSettings = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.tlpControls.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlTopControls.SuspendLayout();
            this.SuspendLayout();
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
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 1000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // tlpControls
            // 
            this.tlpControls.ColumnCount = 2;
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpControls.Controls.Add(this.txtLog, 2, 0);
            this.tlpControls.Controls.Add(this.panel2, 0, 1);
            this.tlpControls.Controls.Add(this.pnlTopControls, 0, 0);
            this.tlpControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpControls.Location = new System.Drawing.Point(0, 0);
            this.tlpControls.Name = "tlpControls";
            this.tlpControls.RowCount = 2;
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpControls.Size = new System.Drawing.Size(759, 416);
            this.tlpControls.TabIndex = 19;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnMeeting);
            this.panel2.Controls.Add(this.txtDummyMode);
            this.panel2.Controls.Add(this.btnStart);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 288);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(144, 125);
            this.panel2.TabIndex = 25;
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(15, 91);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(114, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Отменить операцию";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnMeeting
            // 
            this.btnMeeting.Location = new System.Drawing.Point(15, 62);
            this.btnMeeting.Name = "btnMeeting";
            this.btnMeeting.Size = new System.Drawing.Size(114, 23);
            this.btnMeeting.TabIndex = 18;
            this.btnMeeting.Text = "Митинг";
            this.btnMeeting.UseVisualStyleBackColor = true;
            // 
            // txtDummyMode
            // 
            this.txtDummyMode.AutoSize = true;
            this.txtDummyMode.Location = new System.Drawing.Point(15, 10);
            this.txtDummyMode.Name = "txtDummyMode";
            this.txtDummyMode.Size = new System.Drawing.Size(114, 17);
            this.txtDummyMode.TabIndex = 17;
            this.txtDummyMode.Text = "Тестовый прогон";
            this.txtDummyMode.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(15, 33);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(114, 23);
            this.btnStart.TabIndex = 16;
            this.btnStart.Text = "Проставить время";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // pnlTopControls
            // 
            this.pnlTopControls.Controls.Add(this.btnDoForDate);
            this.pnlTopControls.Controls.Add(this.dteForDay);
            this.pnlTopControls.Controls.Add(this.btnSettings);
            this.pnlTopControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopControls.Location = new System.Drawing.Point(3, 3);
            this.pnlTopControls.Name = "pnlTopControls";
            this.pnlTopControls.Size = new System.Drawing.Size(144, 125);
            this.pnlTopControls.TabIndex = 24;
            // 
            // btnDoForDate
            // 
            this.btnDoForDate.Location = new System.Drawing.Point(15, 84);
            this.btnDoForDate.Name = "btnDoForDate";
            this.btnDoForDate.Size = new System.Drawing.Size(114, 36);
            this.btnDoForDate.TabIndex = 25;
            this.btnDoForDate.Text = "Проставить на дату";
            this.btnDoForDate.UseVisualStyleBackColor = true;
            // 
            // dteForDay
            // 
            this.dteForDay.CustomFormat = "dd.MM.yyyy";
            this.dteForDay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dteForDay.Location = new System.Drawing.Point(15, 57);
            this.dteForDay.MinDate = new System.DateTime(2019, 1, 1, 0, 0, 0, 0);
            this.dteForDay.Name = "dteForDay";
            this.dteForDay.ShowUpDown = true;
            this.dteForDay.Size = new System.Drawing.Size(114, 20);
            this.dteForDay.TabIndex = 24;
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(15, 4);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(114, 23);
            this.btnSettings.TabIndex = 23;
            this.btnSettings.Text = "Настройки";
            this.btnSettings.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(153, 3);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.tlpControls.SetRowSpan(this.txtLog, 2);
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(603, 410);
            this.txtLog.TabIndex = 27;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 416);
            this.Controls.Add(this.tlpControls);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(775, 455);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Бот логирования времени";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.tlpControls.ResumeLayout(false);
            this.tlpControls.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlTopControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon ntfyIcon;
        private System.Windows.Forms.Timer tmrStart;
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.TableLayoutPanel tlpControls;
        private System.Windows.Forms.Panel pnlTopControls;
        private System.Windows.Forms.Button btnDoForDate;
        private System.Windows.Forms.DateTimePicker dteForDay;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnMeeting;
        private System.Windows.Forms.CheckBox txtDummyMode;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtLog;
    }
}

