namespace ValidateTerminal
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            label1 = new Label();
            txtList = new TextBox();
            btnSubmit = new Button();
            dataGridViewResults = new DataGridView();
            lblCount = new Label();
            lblGrid = new Label();
            btnDelete = new Button();
            lblStatus = new Label();
            btnGo = new Button();
            lblDebug = new Label();
            btnBack = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewResults).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(165, 15);
            label1.TabIndex = 0;
            label1.Text = "Введите номера терминалов";
            // 
            // txtList
            // 
            txtList.Location = new Point(15, 33);
            txtList.Multiline = true;
            txtList.Name = "txtList";
            txtList.ScrollBars = ScrollBars.Vertical;
            txtList.Size = new Size(308, 251);
            txtList.TabIndex = 1;
            txtList.TextChanged += txtList_TextChanged_1;
            txtList.KeyDown += txtList_KeyDown;
            // 
            // btnSubmit
            // 
            btnSubmit.Location = new Point(-1, 375);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(75, 23);
            btnSubmit.TabIndex = 2;
            btnSubmit.Text = "Отправить";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // dataGridViewResults
            // 
            dataGridViewResults.AllowUserToAddRows = false;
            dataGridViewResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewResults.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewResults.Location = new Point(329, 0);
            dataGridViewResults.Name = "dataGridViewResults";
            dataGridViewResults.Size = new Size(778, 593);
            dataGridViewResults.TabIndex = 3;
            // 
            // lblCount
            // 
            lblCount.AutoSize = true;
            lblCount.Location = new Point(15, 296);
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(0, 15);
            lblCount.TabIndex = 4;
            // 
            // lblGrid
            // 
            lblGrid.AutoSize = true;
            lblGrid.Location = new Point(15, 311);
            lblGrid.Name = "lblGrid";
            lblGrid.Size = new Size(0, 15);
            lblGrid.TabIndex = 5;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(80, 375);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 23);
            btnDelete.TabIndex = 6;
            btnDelete.Text = "Очистить";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.BackColor = Color.Transparent;
            lblStatus.Location = new Point(15, 326);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 15);
            lblStatus.TabIndex = 7;
            // 
            // btnGo
            // 
            btnGo.Location = new Point(161, 375);
            btnGo.Name = "btnGo";
            btnGo.Size = new Size(162, 23);
            btnGo.TabIndex = 8;
            btnGo.Text = "Движение терминалов";
            btnGo.UseVisualStyleBackColor = true;
            btnGo.Click += btnGo_Click;
            // 
            // lblDebug
            // 
            lblDebug.AutoSize = true;
            lblDebug.Location = new Point(600, 504);
            lblDebug.Name = "lblDebug";
            lblDebug.Size = new Size(0, 15);
            lblDebug.TabIndex = 9;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(0, 410);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 23);
            btnBack.TabIndex = 10;
            btnBack.Text = "Назад";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(1109, 593);
            Controls.Add(btnBack);
            Controls.Add(lblDebug);
            Controls.Add(btnGo);
            Controls.Add(lblStatus);
            Controls.Add(btnDelete);
            Controls.Add(lblGrid);
            Controls.Add(lblCount);
            Controls.Add(dataGridViewResults);
            Controls.Add(btnSubmit);
            Controls.Add(txtList);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "Валидация терминалов";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewResults).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtList;
        private Button btnSubmit;
        private DataGridView dataGridViewResults;
        private Label lblCount;
        private Label lblGrid;
        private Button btnDelete;
        private Label lblStatus;
        private Button btnGo;
        private Label lblDebug;
        private Button btnBack;
    }
}