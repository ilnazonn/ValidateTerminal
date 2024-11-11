using System;
using System.Windows.Forms;

namespace ValidateTerminal // Замените на имя вашего пространства имен
{
    static class Program
    {
        [STAThread]
        [Obsolete]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new validateForm()); // Замените LoginForm на имя вашей формы

        }
    }
}
