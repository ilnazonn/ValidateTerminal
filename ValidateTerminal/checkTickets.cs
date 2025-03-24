using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ValidateTerminal
{
    public partial class checkTickets : Form
    {
        private string token;
        private WebBrowser webBrowserList; // Объявляем элемент управления
        private DateTime lastCommentCreateTime;
        private System.Windows.Forms.Timer trackingTimer;
        private Dictionary<int, DateTime> lastCommentTimes = new Dictionary<int, DateTime>();
        public checkTickets(string token)
        {
            InitializeComponent();
            this.token = token;
            trackingTimer = new System.Windows.Forms.Timer();
            trackingTimer.Tick += TrackingTimer_Tick;
            interval.Text = "5"; // Установка значения по умолчанию
            // Подписываемся на событие закрытия формы
            ApplicationManager.SubscribeToFormClosing(this);

            // Инициализация webBrowserList
            webBrowserList = new WebBrowser();
            webBrowserList.Dock = DockStyle.Fill; // Растягиваем на всю форму
            this.Controls.Add(webBrowserList); // Добавляем на форму

            // Подписываемся на событие изменения состояния чекбокса
            hideClosedCheckBox.CheckedChanged += HideClosedCheckBox_CheckedChanged;
        }
        private void HideClosedCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (sender == null)
                return;
            // При изменении состояния чекбокса перестраиваем таблицу
            btnApply_Click(sender, e);
        }



        private async void btnApply_Click(object sender, EventArgs e)
        {
            statusInfo.Visible = true;
            statusInfo.Text = "Строится таблица...";
            // Очищаем WebBrowser перед добавлением новых данных
            webBrowserList.DocumentText = string.Empty;

            // Получаем ссылки из textBoxLinks
            var links = textBoxLinks.Text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Проверка на отсутствие данных в textBox
            if (links.Length == 0)
            {
                MessageBox.Show("Ошибка: Введите ссылки на тикеты.");
                statusInfo.Visible = false;
                return;
            }

            // Обновляем статус
            statusInfo.Text = "Строится таблица...";

            // Создаем HTML-таблицу с использованием Bootstrap
            var htmlContent = new StringBuilder();
            htmlContent.Append(@"
<!DOCTYPE html>
<html lang='ru'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Таблица тикетов</title>
    <link href='https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css' rel='stylesheet'>
    <style>
        .table th, .table td {
            vertical-align: middle;
        }
        .table thead th {
            background-color: #343a40;
            color: white;
        }
        .table tbody tr:hover {
            background-color: #f8f9fa;
        }
    </style>
</head>
<body>
    <div class='container mt-4'>
        <h2 class='mb-4'>Таблица тикетов</h2>
        <table class='table table-bordered table-striped'>
            <thead>
                <tr>
                    <th>Тема</th>
                    <th>Дата создания</th>
                    <th>Дата закрытия</th>
                    <th>Дней до закрытия</th>
                    <th>Комментарий</th>
                    <th>Комментарий от</th>
                    <th>Решено за</th>
                </tr>
            </thead>
            <tbody>");

            int errorCount = 0; // Счетчик ошибок
            const int maxErrors = 5; // Максимальное количество ошибок

            foreach (var link in links)
            {
                var ticketId = link.Trim().Split('/').Last();

                if (!int.TryParse(ticketId, out int id))
                {
                    MessageBox.Show($"Ошибка: Некорректная ссылка - {link}");
                    errorCount++;

                    if (errorCount > maxErrors)
                    {
                        MessageBox.Show("Превышено допустимое количество ошибок. Завершение.");
                        break; // Выход из цикла
                    }

                    continue;
                }

                // Получаем информацию о тикете
                var ticketInfo = await GetTicketInfoAsync(id);
                if (ticketInfo == null)
                {
                    MessageBox.Show($"Ошибка: Не удалось получить данные о тикете - {link}");
                    continue;
                }

                // Получаем комментарии
                var comments = await GetCommentsAsync(id);
                if (comments == null)
                {
                    MessageBox.Show($"Ошибка: Не удалось получить комментарии для тикета - {link}");
                    continue;
                }
                // Сохраняем время последнего комментария
                if (comments.items.Count > 0)
                {
                    var lastComment = comments.items[comments.items.Count - 1];
                    var currentCommentCreateTime = DateTime.Parse(lastComment.create_time.ToString());
                    lastCommentTimes[id] = currentCommentCreateTime;
                }

                // Обрабатываем данные и добавляем их в HTML-таблицу
                var ticketRow = ProcessTicketData(ticketInfo, comments, link);

                // Проверяем, нужно ли скрывать закрытые тикеты
                if (hideClosedCheckBox.Checked && ticketRow.Contains("Тикет закрыт"))
                {
                    continue; // Пропускаем закрытые тикеты
                }


                htmlContent.Append(ticketRow);
            }

            // Обновляем статус при завершении
            statusInfo.Visible = false;

            // Скрываем textBoxLinks и btnApply
            textBoxLinks.Visible = false;
            btnApply.Visible = false;
            htmlContent.Append(@"
            </tbody>
        </table>
    </div>
</body>
</html>");

            // Отображаем HTML-код в WebBrowser
            webBrowserList.DocumentText = htmlContent.ToString();
        }

        private async Task<dynamic?> GetTicketInfoAsync(int ticketId)
        {
            using (var client = new HttpClient())
            {
                var url = $"https://api.vendista.ru:99/tickets/{ticketId}/?token={token}";
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<dynamic>(content);
                }
            }

            return null;
        }

        private async Task<dynamic?> GetCommentsAsync(int ticketId)
        {
            using (var client = new HttpClient())
            {
                var url = $"https://api.vendista.ru:99/tickets/{ticketId}/comments?token={token}";
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<dynamic>(content);
                }
            }

            return null;
        }

        private string ProcessTicketData(dynamic ticketInfo, dynamic comments, string link)
        {
            var createTime = ticketInfo.item.create_time;
            var closeTime = ticketInfo.item.close_time;
            var subject = ticketInfo.item.subject; // Извлекаем тему запроса
            var lastComment = comments.items.Count > 0 ? comments.items[comments.items.Count - 1] : null;

            var createDate = DateTime.Parse(createTime.ToString());
            var currentDate = DateTime.Now;
            var closeDate = lastComment != null ? DateTime.Parse(lastComment.create_time.ToString()).AddDays(15) : createDate.AddDays(15);

            var daysToClose = currentDate > closeDate ? "Тикет закрыт" : (closeDate - currentDate).Days.ToString();

            // Определяем цвет в зависимости от дней до закрытия
            string cellColor = "";
            if (daysToClose != "Тикет закрыт")
            {
                int days = int.Parse(daysToClose);
                if (days >= 10 && days <= 15)
                {
                    cellColor = "background-color: #90EE90;"; // Зеленый
                }
                else if (days >= 5 && days <= 9)
                {
                    cellColor = "background-color: #FFFF99;"; // Желтый
                }
                else if (days >= 1 && days <= 4)
                {
                    cellColor = "background-color: #FFCCCB;"; // Красный
                }
            }
            // Проверяем текст последнего комментария
            if (lastComment?.comment != null)
            {
                string commentText = lastComment.comment.ToString();
                if (commentText.Contains("Заявка закрылась автоматически так как не было ответа от клиента в течении 15 дней"))
                {
                    daysToClose = "Тикет закрыт"; // Переопределяем значение
                }
            }

            var daysToResolve = "-";
            if (createTime != null && closeTime != null) // Проверяем на null
            {
                // Объявляем resolveDate как DateTime и используем TryParse
                if (DateTime.TryParse(closeTime?.ToString(), out DateTime resolveDate)) // Используем ?. для проверки на null
                {
                    daysToResolve = (resolveDate - createDate).Days.ToString();
                }
            }




            var commentFrom = lastComment != null ? lastComment.user_name.ToString() : "-";

            // Формируем строку таблицы
            return $@"
<tr>
    <td><a href='{link}' target='_blank'>{subject}</a></td> <!-- Тема с ссылкой -->
    <td>{createDate:dd.MM.yyyy HH:mm:ss}</td>
    <td>{(closeTime != null ? DateTime.Parse(closeTime.ToString()).ToString("dd.MM.yyyy HH:mm:ss") : "-")}</td>
    <td style='{cellColor}'>{daysToClose}</td>
    <td>{(lastComment != null ? lastComment.comment.ToString() : "Нет комментариев")}</td>
    <td>{commentFrom}</td>
    <td>{daysToResolve}</td>
</tr>";
        }

        private void btnOpenList_Click(object sender, EventArgs e)
        {
            // Переключаем видимость элементов textBoxLinks и btnApply
            textBoxLinks.Visible = !textBoxLinks.Visible; // Переключаем видимость
            btnApply.Visible = !btnApply.Visible; // Переключаем видимость
        }

        private void btnback_Click(object sender, EventArgs e)
        {
            ChooseFunctional functionalForm = new ChooseFunctional(token); // Передаем токен в конструктор
            functionalForm.Show(); // Показываем новую форму
            this.Hide(); // Скрываем текущую форму

        }

        private async Task UpdateLastCommentTimes()
        {
            var links = textBoxLinks.Text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var link in links)
            {
                var ticketId = link.Trim().Split('/').Last();

                if (int.TryParse(ticketId, out int id))
                {
                    var comments = await GetCommentsAsync(id);
                    if (comments != null && comments.items.Count > 0)
                    {
                        var lastComment = comments.items[comments.items.Count - 1];
                        var currentCommentCreateTime = DateTime.Parse(lastComment.create_time.ToString());
                        lastCommentTimes[id] = currentCommentCreateTime;
                    }
                }
            }
        }

        private async void chkTracking_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTracking.Checked)
            {
                if (webBrowserList.DocumentText == string.Empty)
                {
                    MessageBox.Show("Сначала постройте таблицу тикетов.");
                    chkTracking.Checked = false;
                    return;
                }

                // Обновляем время последнего комментария
                await UpdateLastCommentTimes();

                if (int.TryParse(interval.Text, out int intervalMinutes))
                {
                    trackingTimer.Interval = intervalMinutes * 60 * 1000; // Переводим минуты в миллисекунды
                    trackingTimer.Start();
                }
                else
                {
                    MessageBox.Show("Некорректное значение интервала.");
                    chkTracking.Checked = false;
                }
            }
            else
            {
                trackingTimer.Stop();
            }
        }

        private void interval_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(interval.Text, out int intervalMinutes))
            {
                trackingTimer.Interval = intervalMinutes * 60 * 1000; // Переводим минуты в миллисекунды
            }
        }

        private void ShowNotification(string message)
        {
            var notification = new NotifyIcon
            {
                Visible = true,
                Icon = SystemIcons.Information,
                BalloonTipText = message,
                BalloonTipTitle = "Изменение комментария"
            };

            notification.ShowBalloonTip(5000); // Показываем уведомление на 5 секунд
        }

private async void TrackingTimer_Tick(object sender, EventArgs e)
{
    var links = textBoxLinks.Text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

    foreach (var link in links)
    {
        var ticketId = link.Trim().Split('/').Last();

        if (int.TryParse(ticketId, out int id))
        {
            // Получаем актуальные комментарии из API
            var comments = await GetCommentsAsync(id);
            if (comments != null && comments.items.Count > 0)
            {
                var lastComment = comments.items[comments.items.Count - 1];
                var currentCommentCreateTime = DateTime.Parse(lastComment.create_time.ToString());

                // Проверяем, есть ли запись о времени последнего комментария для этого тикета
                if (lastCommentTimes.ContainsKey(id))
                {
                    // Если время последнего комментария изменилось
                    if (currentCommentCreateTime > lastCommentTimes[id])
                    {
                        // Обновляем время последнего комментария
                        lastCommentTimes[id] = currentCommentCreateTime;

                        // Показываем уведомление
                        ShowNotification($"Комментарий в задаче {id} был изменен.");
                        System.Media.SystemSounds.Beep.Play(); // Звуковое уведомление

                        // Обновляем таблицу (опционально)
                        btnApply_Click(null, null);
                    }
                }
                else
                {
                    // Если это первый раз, сохраняем время последнего комментария
                    lastCommentTimes[id] = currentCommentCreateTime;
                }
            }
        }
    }
}



    }
}