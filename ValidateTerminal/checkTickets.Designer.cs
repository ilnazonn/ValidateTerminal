﻿namespace ValidateTerminal
{
    partial class checkTickets
    {

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(checkTickets));
            textBoxLinks = new TextBox();
            btnApply = new Button();
            statusInfo = new Label();
            hideClosedCheckBox = new CheckBox();
            btnback = new Button();
            btnOpenList = new Button();
            panel1 = new Panel();
            chkTracking = new CheckBox();
            interval = new TextBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // textBoxLinks
            // 
            textBoxLinks.Location = new Point(0, 39);
            textBoxLinks.Multiline = true;
            textBoxLinks.Name = "textBoxLinks";
            textBoxLinks.Size = new Size(250, 150);
            textBoxLinks.TabIndex = 0;
            // 
            // btnApply
            // 
            btnApply.Location = new Point(0, 207);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(75, 23);
            btnApply.TabIndex = 1;
            btnApply.Text = "Проверить";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            // 
            // statusInfo
            // 
            statusInfo.AutoSize = true;
            statusInfo.Location = new Point(0, 14);
            statusInfo.Name = "statusInfo";
            statusInfo.Size = new Size(0, 15);
            statusInfo.TabIndex = 2;
            // 
            // hideClosedCheckBox
            // 
            hideClosedCheckBox.AutoSize = true;
            hideClosedCheckBox.Location = new Point(121, 13);
            hideClosedCheckBox.Name = "hideClosedCheckBox";
            hideClosedCheckBox.Size = new Size(123, 19);
            hideClosedCheckBox.TabIndex = 3;
            hideClosedCheckBox.Text = "Скрыть закрытые";
            hideClosedCheckBox.UseVisualStyleBackColor = true;
            // 
            // btnback
            // 
            btnback.BackgroundImageLayout = ImageLayout.None;
            btnback.Location = new Point(250, 9);
            btnback.Name = "btnback";
            btnback.Size = new Size(75, 23);
            btnback.TabIndex = 4;
            btnback.Text = "Назад";
            btnback.UseVisualStyleBackColor = true;
            btnback.Click += btnback_Click;
            // 
            // btnOpenList
            // 
            btnOpenList.Location = new Point(336, 9);
            btnOpenList.Name = "btnOpenList";
            btnOpenList.Size = new Size(156, 23);
            btnOpenList.TabIndex = 5;
            btnOpenList.Text = "Открыть\\Закрыть форму";
            btnOpenList.UseVisualStyleBackColor = true;
            btnOpenList.Click += btnOpenList_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(interval);
            panel1.Controls.Add(chkTracking);
            panel1.Controls.Add(statusInfo);
            panel1.Controls.Add(btnOpenList);
            panel1.Controls.Add(hideClosedCheckBox);
            panel1.Controls.Add(btnback);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1109, 39);
            panel1.TabIndex = 6;
            // 
            // chkTracking
            // 
            chkTracking.AutoSize = true;
            chkTracking.Location = new Point(498, 9);
            chkTracking.Name = "chkTracking";
            chkTracking.Size = new Size(163, 19);
            chkTracking.TabIndex = 6;
            chkTracking.Text = "Включить отслеживание";
            chkTracking.UseVisualStyleBackColor = true;
            // 
            // interval
            // 
            interval.Enabled = false;
            interval.Location = new Point(658, 7);
            interval.Name = "interval";
            interval.Size = new Size(48, 23);
            interval.TabIndex = 7;
            // 
            // checkTickets
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(1109, 593);
            Controls.Add(panel1);
            Controls.Add(btnApply);
            Controls.Add(textBoxLinks);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "checkTickets";
            Text = "Проверка тикетов";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxLinks;
        private Button btnApply;
        private Label statusInfo;
        private CheckBox hideClosedCheckBox;
        private Button btnback;
        private Button btnOpenList;
        private Panel panel1;
        private CheckBox chkTracking;
        private TextBox interval;
    }
}