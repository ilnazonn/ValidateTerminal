namespace ValidateTerminal
{
    partial class ChooseFunctional
    {
        private string token;
        public ChooseFunctional(string token)
        {
            InitializeComponent(); // Инициализация компонентов формы

            this.token = token; // Сохраняем токен
        }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseFunctional));
            btnCheckTerminals = new Button();
            btnCheckTicket = new Button();
            SuspendLayout();
            // 
            // btnCheckTerminals
            // 
            btnCheckTerminals.Location = new Point(420, 237);
            btnCheckTerminals.Name = "btnCheckTerminals";
            btnCheckTerminals.Size = new Size(153, 23);
            btnCheckTerminals.TabIndex = 0;
            btnCheckTerminals.Text = "Проверка терминалов";
            btnCheckTerminals.UseVisualStyleBackColor = true;
            btnCheckTerminals.Click += btnCheckTerminals_Click;
            // 
            // btnCheckTicket
            // 
            btnCheckTicket.Location = new Point(420, 273);
            btnCheckTicket.Name = "btnCheckTicket";
            btnCheckTicket.Size = new Size(153, 23);
            btnCheckTicket.TabIndex = 1;
            btnCheckTicket.Text = "Проверка тикетов";
            btnCheckTicket.UseVisualStyleBackColor = true;
            btnCheckTicket.Click += btnCheckTicket_Click;
            // 
            // ChooseFunctional
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(1109, 593);
            Controls.Add(btnCheckTicket);
            Controls.Add(btnCheckTerminals);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ChooseFunctional";
            Text = "Выбор функциональности";
            ResumeLayout(false);
        }

        #endregion

        private Button btnCheckTerminals;
        private Button btnCheckTicket;
    }
}