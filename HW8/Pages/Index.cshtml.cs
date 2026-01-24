using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HW8.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            ViewData["Title"] = "Крупные города России";

            ViewData["Cities"] = new List<string>
            {
                "Казань",
                "Москва",
                "Уфа",
                "Владимир",
                "Калининград",
                "Санкт-Петербург",
                "Екатеринбург",
                "Нижний Новгород",
                "Новосибирск",
                "Краснодар"
            };
        }
    }
}
