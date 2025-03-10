using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ValidateTerminal
{
    public partial class ChooseFunctional : Form
    {
        public ChooseFunctional()
        {
            InitializeComponent();
        }

        private void btnCheckTerminals_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm(token); // Передаем токен в конструктор
            mainForm.Show(); // Показываем новую форму
            this.Hide(); // Скрываем текущую форму
        }

        private void btnCheckTicket_Click(object sender, EventArgs e)
        {
            checkTickets checkTicketsForm = new checkTickets(token);  // Предположим, что token был объявлен в ChooseFunctional
            checkTicketsForm.Show(); // Показываем новую форму
            this.Hide(); // Скрываем текущую форму
        }
    }
}
