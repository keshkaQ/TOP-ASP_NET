//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.MapGet("/convert", (string usd) => ConvertUsdToRub(usd));

//static string ConvertUsdToRub(string usdInput)
//{
//    const double k = 78.23;

//    if (string.IsNullOrWhiteSpace(usdInput))
//    {
//        return "Пожалуйста, укажите сумму в USD";
//    }
//    if (!double.TryParse(usdInput, out double usd))
//    {
//        return "Пожалуйста, введите корректное число";
//    }

//    if (usd <= 0)
//    {
//        return "Сумма должна быть положительным числом";
//    }

//    double rubles = usd * k;

//    return $"Вы ввели: {usd:F2} долларов\nПолученные рубли: {rubles:F2}";
//}

//app.Run();

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Введите параметр name в адресе /hello?name=Вася");

// дополнительный параметр
app.MapGet("/hello", Hello);
// проверка по https://localhost:7056/hello?name=Вася

app.Run();

IResult Hello(string name, HttpContext context)
{
    // context.Response.ContentType = "text/html; charset=utf-8";
    var html = $"<h1>Привет {name} !</h1>";
    // return Results.Text(html);
    return Results.Text(html, "text/html; charset=utf-8");
}
