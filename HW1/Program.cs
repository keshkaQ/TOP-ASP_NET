var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/convert", (string usd) => ConvertUsdToRub(usd));

static string ConvertUsdToRub(string usdInput)
{
    const double k = 78.23;

    if (string.IsNullOrWhiteSpace(usdInput))
    {
        return "Пожалуйста, укажите сумму в USD";
    }
    if (!double.TryParse(usdInput, out double usd))
    {
        return "Пожалуйста, введите корректное число";
    }

    if (usd <= 0)
    {
        return "Сумма должна быть положительным числом";
    }

    double rubles = usd * k;

    return $"Вы ввели: {usd:F2} долларов\nПолученные рубли: {rubles:F2}";
}

app.Run();