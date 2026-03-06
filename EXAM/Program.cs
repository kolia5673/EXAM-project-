using OfficeOpenXml;
using Serilog;
// Видаліть using System.ComponentModel; якщо він є

ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

var builder = WebApplication.CreateBuilder(args);

// Додайте Serilog, якщо потрібно
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

string? excelPath = null;

Console.WriteLine("====================================");
Console.WriteLine(" PRODUCT SERVER ");
Console.WriteLine("====================================");

Console.Write("Введите путь к Excel файлу: ");
excelPath = Console.ReadLine();

if (!File.Exists(excelPath))
{
    Console.WriteLine("Файл не найден!");
    return; // Завершуємо програму, якщо файл не знайдено
}
else
{
    Console.WriteLine("Excel файл успешно загружен");
}

app.MapGet("/health", () =>
{
    Console.WriteLine("[INFO] Проверка состояния сервера");
    return Results.Ok(new
    {
        status = "Server working",
        time = DateTime.Now
    });
});

app.MapPost("/products/search", async (List<string> names) =>
{
    Console.WriteLine("====================================");
    Console.WriteLine("[REQUEST] Поиск товаров");
    Console.WriteLine("Товары: " + string.Join(", ", names));

    if (excelPath == null || !File.Exists(excelPath))
    {
        Console.WriteLine("[ERROR] Excel файл не найден");
        return Results.BadRequest("Excel файл не найден");
    }

    var results = new List<object>();
    double total = 0;

    using var package = new ExcelPackage(new FileInfo(excelPath));
    var sheet = package.Workbook.Worksheets[0];

    int rows = sheet.Dimension.Rows;

    for (int i = 2; i <= rows; i++)
    {
        var name = sheet.Cells[i, 1].Text;

        // Використовуємо TryParse для уникнення помилок
        if (!double.TryParse(sheet.Cells[i, 2].Text, out double price))
            price = 0;

        if (!int.TryParse(sheet.Cells[i, 3].Text, out int quantity))
            quantity = 0;

        if (names.Contains(name, StringComparer.OrdinalIgnoreCase))
        {
            double sum = price * quantity;

            results.Add(new
            {
                Name = name,
                Price = price,
                Quantity = quantity,
                Sum = sum
            });

            total += sum;

            Console.WriteLine($"[FOUND] {name} | Цена: {price} | Кол-во: {quantity}");
        }
    }

    Console.WriteLine("[INFO] Общая сумма: " + total);

    return Results.Ok(new
    {
        products = results,
        total = total
    });
});

app.Run("http://0.0.0.0:5000");