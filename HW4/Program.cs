var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

var app = builder.Build();

app.MapStaticAssets(); // для статических файлов из wwwroot (css,js,jpg,txt,pdf)
app.MapRazorPages()
   .WithStaticAssets(); // расшириряет маршрутизация Razor Pages с возможностью обслуживать статические фыйлы
app.Run();
