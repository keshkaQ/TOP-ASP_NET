using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HW6.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<Site> Sites { get; private set; } = [];
        public List<Site> DisplayedSites { get; private set; } = [];
        public string? SearchMessage { get; private set; } 

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            Sites = new List<Site>()
            {
                new Site("Google", 90),
                new Site("YouTube", 94),
                new Site("GitHub", 85),
                new Site("Stack Overflow", 76),
                new Site("Vk", 62),
                new Site("Yandex", 89),
            };
        }

        public void OnGet()
        {
            DisplayedSites = Sites;
        }

        public IActionResult OnGetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                SearchMessage = "Введите название сайта для поиска";
                return Page();
            }

            DisplayedSites = Sites.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (DisplayedSites.Count == 0)
                SearchMessage = $"Сайты с названием '{name}' не найдены";
            else
                SearchMessage = $"Найдено {DisplayedSites.Count} сайт(ов)";
            return Page();
        }

        public IActionResult OnGetByRating(int rating)
        {
            if(rating <= 0)
            {
                SearchMessage = "Введите рейтинг для поиска (положительное число)";
                return Page();
            }
                
            DisplayedSites = Sites.Where(p => p.Rating == rating).ToList();

            if (DisplayedSites.Count == 0)
                SearchMessage = $"Сайты с рейтингом {rating} не найдены";
            else
                SearchMessage = $"Найдено {DisplayedSites.Count} сайт(ов) с рейтингом {rating}";
            return Page();
        }
    }

    public record Site(string Name, int Rating);
}