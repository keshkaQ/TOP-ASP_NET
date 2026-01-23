using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace HW7.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string? Result { get; set; }
        public string? AuthorName { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet(string? name, string? format = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Result = "Введите имя автора в URL: /?name=Пушкин";
                return Page();
            }
            AuthorName = name;

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{name}.txt");

            if (System.IO.File.Exists(filePath))
            {
                string content = System.IO.File.ReadAllText(filePath, Encoding.UTF8);
                switch(format?.ToLower())
                {
                    case "json":
                        var jsonData = new
                        {
                            Author = name,
                            FileName = $"{name}.txt",
                            Content = content,
                        };

                        string jsonString = JsonSerializer.Serialize(jsonData,
                            new JsonSerializerOptions
                            {
                                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                                WriteIndented = true
                            });

                        return Content(jsonString, "application/json; charset=utf-8");
                    case "file":
                        {
                            var fileBytes = System.IO.File.ReadAllBytes(filePath);
                            return File(fileBytes, "text/plain", $"{name}.txt");
                        }
                    default:
                        Result = content;
                        return Page();
                }
            }
            return Redirect($"https://yandex.ru/search/?text={Uri.EscapeDataString(name)}");
        }
    }
}
