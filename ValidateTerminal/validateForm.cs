using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace ValidateTerminal
{
    public partial class validateForm : Form
    {
public validateForm()
        {
            InitializeComponent();
            Console.WriteLine("Приложение завершает работу...");
            this.Load += Form1_Load; // Можно использовать метод без new EventHandler для сокращения
    // Подписываемся на событие закрытия формы
    ApplicationManager.SubscribeToFormClosing(this);
}

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Text;

            string url = $"https://api.vendista.ru:99/token?login={login}&password={password}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    // Получаем строку ответа
                    string json = await response.Content.ReadAsStringAsync();

                    // Парсим JSON и извлекаем токен
                    var jsonObject = JObject.Parse(json);
                    string? token = jsonObject["token"]?.ToString(); // Получаем значение токена

                    if (string.IsNullOrEmpty(token))
                    {
                        throw new Exception("Токен не найден в ответе.");
                    }

                    // Проверка успешного получения токена
                    status.Text = "Авторизация успешна. Токен: "; // Отображаем токен в статусе
                    status.ForeColor = System.Drawing.Color.Green;

                    if (rememberMe.Checked)
                    {
                        // Сохраняем логин и пароль
                        Properties.Settings.Default.Login = login;
                        Properties.Settings.Default.Password = password;
                        Properties.Settings.Default.RememberMe = true;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        // Очищаем сохраненные данные
                        Properties.Settings.Default.Login = string.Empty;
                        Properties.Settings.Default.Password = string.Empty;
                        Properties.Settings.Default.RememberMe = false;
                        Properties.Settings.Default.Save();
                    }

                    // Задержка на 2 секунды для отображения сообщения
                    await Task.Delay(500);

                    // Создание экземпляра MainForm, передавая токен
                    ChooseFunctional functionalForm = new ChooseFunctional(token); // Передаем токен в конструктор
                    functionalForm.Show(); // Показываем новую форму

                    this.Hide(); // Скрываем текущую форму
                }
                catch (Exception ex)
                {
                    status.Text = "Ошибка авторизации: " + ex.Message;
                    status.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        private void Form1_Load(object? sender, EventArgs e)
        {
            // Проверяем, установлена ли опция "Запомнить меня"
            if (Properties.Settings.Default.RememberMe) // Убедитесь, что это свойство существует в ваших настройках
            {
                // Заполняем текстовые поля логина и пароля сохранёнными значениями
                var savedLogin = Properties.Settings.Default.Login;
                var savedPassword = Properties.Settings.Default.Password;

                // Проверяем на null перед присвоением
                if (savedLogin != null)
                {
                    txtLogin.Text = savedLogin; // Заполнение текстового поля логина
                }

                if (savedPassword != null)
                {
                    txtPassword.Text = savedPassword; // Заполнение текстового поля пароля
                }

                // Устанавливаем чекбокс "Запомнить меня" в состояние "проверено"
                rememberMe.Checked = true;
            }
        }


        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Проверяем, была ли нажата клавиша Enter
            {
                btnLogin.PerformClick(); // Вызываем событие нажатия кнопки
            }
        }

    }
}




