using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Net.Http;
using System.Data;
using Newtonsoft.Json.Linq;


namespace ValidateTerminal
{
    public partial class MainForm : Form
    {
        private string? token;


        private void txtList_TextChanged(object sender, EventArgs e)
        {
            CountObjects(); // Вызываем функцию для подсчета
        }

        private Stack<string> previousStates = new Stack<string>();

        private void txtList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z) // Если Ctrl + Z
            {
                e.SuppressKeyPress = true; // Отменяем стандартное поведение

                if (previousStates.Count > 0)
                {
                    // Восстанавливаем предыдущее состояние
                    txtList.Text = previousStates.Pop(); // Удаляем последнее состояние из стека
                }
            }
            else if (e.Control && e.KeyCode == Keys.V) // Если Ctrl + V
            {
                e.SuppressKeyPress = true; // Отменяем стандартную вставку

                // Сохраняем текущее состояние перед вставкой
                previousStates.Push(txtList.Text);

                // Получаем текст из буфера обмена
                string clipboardText = Clipboard.GetText();

                // Явно указываем массив строк
                string[] delimiters = new string[] { "\r\n", "\r", "\n", "," };
                string[] lines = clipboardText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                // Очищаем текущий текст перед вставкой
                txtList.Clear();

                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        txtList.AppendText(line.Trim() + Environment.NewLine); // Убираем лишние пробелы
                    }
                }
            }
        }

        private void CountRowsInGrid()
        {
            // Получаем количество строк в DataGridView
            int rowCount = dataGridViewResults.Rows.Count;

            // Отображаем количество в Label
            lblGrid.Text = $"Количество строк: {rowCount}";
        }

        private void CountObjects()
        {
            // Получаем текст из TextBox
            string text = txtList.Text;

            // Разбиваем текст на строки, убирая пустые строки
            string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            // Получаем количество строк
            int count = lines.Length;

            // Выводим количество в Label
            lblCount.Text = $"Количество объектов: {count}";
        }

        private void txtList_TextChanged_1(object sender, EventArgs e)
        {
            CountObjects();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            txtList.Clear(); // Очищает содержимое TextBox
        }

        public MainForm(string apiToken)
        {
            InitializeComponent();
            token = apiToken ?? throw new ArgumentNullException(nameof(apiToken));
            btnGo.Enabled = false; // Initially disabled
            InitializeMainDataTable(); // Инициализация основного DataTable
            ApplicationManager.SubscribeToFormClosing(this);
        }

        private DataTable mainDataTable = new DataTable();

        private void InitializeMainDataTable()
        {
            // Создание столбцов основного DataTable
            mainDataTable.Columns.Add("Номер терминала");
            mainDataTable.Columns.Add("Клиент");
            mainDataTable.Columns.Add("Дата последнего переноса");
            mainDataTable.Columns.Add("Кому перенесен");
            mainDataTable.Columns.Add("Кем перенесен");

            // Устанавливаем DataGridView на источник данных
            dataGridViewResults.DataSource = mainDataTable;
        }

        private void dataGridViewResults_DataSourceChanged(object sender, EventArgs e)
        {
            // Enable btnGo only if there are rows in the DataGridView
            btnGo.Enabled = dataGridViewResults.Rows.Count > 0;
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            btnSubmit.Enabled = true;
            btnSubmit.Enabled = false;
            UpdateStatus("Ожидаем данных", SystemColors.Control);
            string text = txtList.Text;
            string[] terminals = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            UpdateStatus("Запрос выполняется", Color.Yellow);
            mainDataTable.Clear();

            foreach (string termId in terminals)
            {
                await ProcessTerminalId(termId);
            }

            UpdateStatus("Запрос выполнен", Color.LightGreen);



            btnSubmit.Enabled = true;
            btnGo.Enabled = true;
            CountRowsInGrid();
            // Ждем 5 секунд
            await Task.Delay(5000);

            // Сбрасываем статус
            lblStatus.Text = string.Empty;
            lblStatus.BackColor = SystemColors.Control; // или любой другой цвет по умолчанию
        }

        private async Task ProcessTerminalId(string termId)
        {
            if (int.TryParse(termId, out int id) && termId.Length <= 8)
            {
                string apiUrl = $"https://api.vendista.ru:99/terminals/{termId}?token={token}";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        string response = await client.GetStringAsync(apiUrl);
                        var data = JObject.Parse(response);
                        var ownerId = data?["item"]?["owner_id"]?.ToString();
                        string ownerName = "Имя не найдено";

                        if (ownerId != null)
                        {
                            string ownerApiUrl = $"https://api.vendista.ru:99/owners/{ownerId}?token={token}";
                            string ownerResponse = await client.GetStringAsync(ownerApiUrl);
                            var ownerData = JObject.Parse(ownerResponse);
                            ownerName = ownerData?["name"]?.ToString() ?? ownerName;
                        }

                        // Добавляем результаты в основную таблицу
                        mainDataTable.Rows.Add(termId, ownerName, null, null, null);
                    }
                    catch (Exception)
                    {
                        mainDataTable.Rows.Add(termId, "Failed to fetch data", null, null, null);
                    }
                }
            }
            else
            {
                mainDataTable.Rows.Add(termId, "Invalid terminal ID", null, null, null);
            }
        }

        private void UpdateStatus(string message, Color backColor)
        {
            lblStatus.Text = message;
            lblStatus.BackColor = backColor;
        }

        private async void btnGo_Click(object sender, EventArgs e)
        {
            // Запускаем поиск по всем терминалам в DataGridView
            await FetchOwnerChangeReports();
        }

        private async Task FetchOwnerChangeReports()
        {
            // Перебираем все строки в DataGridView
            foreach (DataRow row in mainDataTable.Rows)
            {
                string terminalId = row["Номер терминала"].ToString()!;
                await FetchOwnerChangeReport(terminalId);
            }
        }

        private async Task FetchOwnerChangeReport(string terminalId)
        {
            // Обновляем статус перед началом запроса
            UpdateStatus("Запрос выполняется", Color.Yellow);

            string dateTo = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            string apiUrl = $"https://api.vendista.ru:99/terminals/owner_change_report?TerminalIds={terminalId}&token={token}&DateFrom=2024-01-01T00:00:00.649Z&DateTo={dateTo}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync(apiUrl);
                    //MessageBox.Show($"Response: {response}"); // Показать ответ для отладки

                    var data = JObject.Parse(response);
                    if (data["success"]?.Value<bool>() == true)
                    {
                        var items = data["items"];
                        if (items != null && items.HasValues)
                        {
                            // Получаем последний элемент по времени
                            var lastItem = items.Last();

                            // Получаем данные последнего элемента
                            var time = lastItem["time"]?.ToString();
                            var toOwnerId = lastItem["to_owner_id"]?.ToString();
                            var movedUserId = lastItem["moved_user_id"]?.ToString();

                            // Выводим movedUserId для отладки
                            if (!string.IsNullOrEmpty(movedUserId))
                            {
                                //MessageBox.Show($"Полученный идентификатор пользователя: {movedUserId}");
                            }

                            // Запрашиваем имя нового владельца и имя пользователя
                            string toOwnerName = await FetchOwnerName(toOwnerId!);
                            string movedUserName = await FetchUserName(movedUserId!);

                            // Выводим полученные данные для отладки
                            //MessageBox.Show($"Полученные данные: \nВремя: {time}\nКому: {toOwnerName}\nКем: {movedUserName}");

                            // Обновляем строку с результатами
                            var row = mainDataTable.AsEnumerable()
                                                    .FirstOrDefault(r => r["Номер терминала"].ToString() == terminalId);

                            if (row != null && !string.IsNullOrEmpty(time))
                            {
                                row["Дата последнего переноса"] = time;
                                row["Кому перенесен"] = toOwnerName; // Записываем имя владельца
                                row["Кем перенесен"] = movedUserName; // Записываем имя пользователя

                                // Убедитесь, что DataGridView обновляет свое отображение
                                dataGridViewResults.Refresh();
                            }
                        }
                        else
                        {
                            //MessageBox.Show("Данные не найдены в ответе.");
                        }
                    }
                    else
                    {
                        //MessageBox.Show("Неуспешный ответ от API.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}");
                }
                finally
                {
                    // Обновляем статус после завершения запроса

                }
            }
            UpdateStatus("Запрос выполнен", Color.LightGreen);
            UpdateStatus(string.Empty, SystemColors.Control);
        }

        private async Task<string> FetchOwnerName(string ownerId)
        {
            if (string.IsNullOrEmpty(ownerId))
            {
                return "Не указано";
            }

            string ownerApiUrl = $"https://api.vendista.ru:99/owners/{ownerId}?token={token}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync(ownerApiUrl);
                    var ownerData = JObject.Parse(response);

                    // Выводим ответ для отладки
                    //MessageBox.Show($"Ответ от API для владельца {ownerId}: {response}");

                    return ownerData?["name"]?.ToString() ?? "Не найдено";
                }
                catch (Exception)
                {
                    return "Ошибка при получении имени владельца";
                }
            }
        }

        private async Task<string> FetchUserName(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return "Не указано";
            }

            string userApiUrl = $"https://api.vendista.ru:99/users/{userId}?token={token}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync(userApiUrl);
                    var userData = JObject.Parse(response);

                    // Отладочная информация: выводим полученный ответ
                    //MessageBox.Show($"Ответ от API для пользователя {userId}: {response}");

                    // Убедимся, что мы правильно извлекаем имя
                    var userName = userData["item"]?["name"]?.ToString(); // Извлекаем имя из вложенного объекта "item"
                    return userName ?? "Не найдено"; // Возвращаем имя или "Не найдено"
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при получении имени пользователя: {ex.Message}");
                    return "Ошибка";
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ChooseFunctional functionalForm = new ChooseFunctional(token); // Передаем токен в конструктор
            functionalForm.Show(); // Показываем новую форму
            this.Hide(); // Закрываем текущую форму
        }
    }
}