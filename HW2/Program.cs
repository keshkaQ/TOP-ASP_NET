using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IMathCalculator,MathCalculator>();
builder.Services.AddScoped<IMathCalculator,TaylorCalculator>();
builder.Services.AddScoped<CalculatorService>();

var app = builder.Build();

app.MapGet("/", () =>
{
    var html = @"
    <h1>Калькулятор функций</h1>
    <h2>Использование:</h2>
    <p>/calc/{function}?x=значение&method=math|taylor</p>
    
    <h3>Примеры:</h3>
    <ul>
        <li><a href='/calc/sin?x=0.5&method=math'>/calc/sin?x=0.5&method=math</a></li>
        <li><a href='/calc/cos?x=0.7&method=taylor'>/calc/cos?x=0.7&method=taylor</a></li>
        <li><a href='/calc/tan?x=0.3'>/calc/tan?x=0.3</a> (по умолчанию math)</li>
        <li><a href='/calc/ln?x=1&method=taylor'>/calc/ln?x=1&method=taylor</a></li>
        <li><a href='/calc/exp?x=1&method=math'>/calc/exp?x=1&method=math</a></li>
    </ul>
    
    <h3>Доступные функции:</h3>
    <ul>
        <li><b>sin</b> - синус</li>
        <li><b>cos</b> - косинус</li>
        <li><b>tan</b> - тангенс</li>
        <li><b>ln</b> - натуральный логарифм</li>
        <li><b>exp</b> - экспонента</li>
    </ul>
    
    <h3>Параметры:</h3>
    <ul>
        <li><b>{function}</b> - имя функции (обязательно)</li>
        <li><b>x</b> - значение аргумента (обязательно)</li>
        <li><b>method</b> - метод: 'math' или 'taylor' (по умолчанию 'math')</li>
    </ul>";
    return Results.Content(html, "text/html; charset=utf-8");
});

app.MapGet("/calc/{function}", (CalculatorService service, string function, double x, string method = "math") =>
{
    try
    {
        var result = function.ToLower() switch
        {
            "sin" => service.CalculateSin(x, method),
            "cos" => service.CalculateCos(x, method),
            "tan" => service.CalculateTan(x, method),
            "ln" => service.CalculateLn(x, method),
            "exp" => service.CalculateExp(x, method),
            _ => throw new ArgumentException($"Неизвестная функция: {function}. " +
                $"Доступные: sin, cos, tan, ln, exp")
        };

        var response = new
        {
            Function = function,
            Argument = x,
            Method = method,
            Result = Math.Round(result, 10)
        };

        return Results.Json(response,new JsonSerializerOptions{WriteIndented = true });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { Error = ex.Message });
    }
});


app.Run();
