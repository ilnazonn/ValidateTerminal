using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ValidateTerminal
{
    public static class ApplicationManager
    {
        // Метод для завершения приложения
        public static void ExitApplication()
        {
            // Здесь можно добавить дополнительную логику, например, сохранение данных
            Console.WriteLine("Приложение завершает работу...");

            // Завершаем приложение
            Application.Exit(); // Корректное завершение приложения
            Process.GetCurrentProcess().Kill();
        }

        // Метод для подписки на событие закрытия формы
        public static void SubscribeToFormClosing(Form form)
        {
            form.FormClosing += (sender, e) =>
            {
                // Вызываем завершение приложения при закрытии формы
                ExitApplication();
            };
        }
    }
}