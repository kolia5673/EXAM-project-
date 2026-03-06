using Client.Models;  // Змінено з ShopClientWinForms.Models
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class MainForm : Form
    {
        private readonly HttpClient client = new HttpClient();

        public MainForm()
        {
            InitializeComponent();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                // Перевірка введених даних
                if (string.IsNullOrWhiteSpace(txtServer.Text))
                {
                    MessageBox.Show("Введіть адресу сервера", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtProducts.Text))
                {
                    MessageBox.Show("Введіть назви товарів", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string server = txtServer.Text.Trim();

                // Додаємо http:// якщо не вказано протокол
                if (!server.StartsWith("http://") && !server.StartsWith("https://"))
                {
                    server = "http://" + server;
                }

                server = server.TrimEnd('/');

                var names = new List<string>();
                foreach (var item in txtProducts.Text.Split(','))
                {
                    string trimmedItem = item.Trim();
                    if (!string.IsNullOrWhiteSpace(trimmedItem))
                    {
                        names.Add(trimmedItem);
                    }
                }

                if (names.Count == 0)
                {
                    MessageBox.Show("Введіть хоча б одну назву товару", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Відключаємо кнопку
                btnSearch.Enabled = false;
                btnSearch.Text = "Пошук...";

                var response = await client.PostAsJsonAsync(server + "/products/search", names);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Помилка сервера: {response.StatusCode}\n{errorMessage}", "Помилка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var data = await response.Content.ReadFromJsonAsync<SearchResponse>();

                if (data == null)
                {
                    MessageBox.Show("Отримано порожню відповідь від сервера", "Помилка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                grid.Rows.Clear();

                if (data.products != null && data.products.Count > 0)
                {
                    double totalSum = 0;

                    foreach (var p in data.products)
                    {
                        totalSum += p.Sum;

                        grid.Rows.Add(
                            p.Name ?? "Без назви",
                            p.Price.ToString("F2"),
                            p.Quantity,
                            p.Sum.ToString("F2")
                        );
                    }

                    lblTotal.Text = $"Загальна сума: {totalSum:F2}";
                    lblTotal.ForeColor = System.Drawing.Color.Blue;
                    lblTotal.Font = new System.Drawing.Font(lblTotal.Font, System.Drawing.FontStyle.Bold);
                }
                else
                {
                    lblTotal.Text = "Товари не знайдено";
                    lblTotal.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Помилка з'єднання з сервером. Перевірте адресу сервера та спробуйте ще раз.",
                    "Помилка з'єднання", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("Час очікування відповіді від сервера вичерпано.",
                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася неочікувана помилка: {ex.Message}",
                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSearch.Enabled = true;
                btnSearch.Text = "Пошук";
            }
        }
    }
}