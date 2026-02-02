using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace HW11.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }

        [BindProperty]
        public Movie Movie { get; set; } = new Movie();

        public IEnumerable<SelectListItem> Genres { get; set; }
        public List<Content> ContentTypes { get; } = new List<Content>
        {
            new() { Value = "Movie", Name = "Фильм" },
            new() { Value = "Series", Name = "Сериал" },
            new() { Value = "MiniSeries", Name = "Мини-сериал" },
            new() { Value = "Animation", Name = "Анимация" }
        };

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            Genres = new List<SelectListItem>
            {
                new SelectListItem { Value = "Comedy", Text = "Комедия" },
                new SelectListItem { Value = "Horror", Text = "Ужасы" },
                new SelectListItem { Value = "Fantasy", Text = "Фантастика" },
                new SelectListItem { Value = "Drama", Text = "Драма" },
                new SelectListItem { Value = "Thriller", Text = "Триллер" },
                new SelectListItem { Value = "Action", Text = "Боевик" },
                new SelectListItem { Value = "Romance", Text = "Мелодрама" },
            };
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                Message = $"Фильм '{Movie.Name}' ({Movie.YearOfRelease}) успешно добавлен!";
                IsSuccess = true;

                Movie = new Movie();
                ModelState.Clear();
            }
            else
            {
                Message = "Пожалуйста, исправьте ошибки в форме";
                IsSuccess = false;
            }

            return Page();
        }
    }

    public class Movie
    {
        [Required(ErrorMessage = "Название обязательно")]
        [Display(Name = "Название")]
        [StringLength(100, ErrorMessage = "Максимальная длина 100 символов")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Описание обязательно")]
        [StringLength(500, ErrorMessage = "Максимальная длина 500 символов")]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Рейтинг обязателен")]
        [Range(1.0, 5.0, ErrorMessage = "Рейтинг фильма может быть от 1.0 до 5.0")]
        [Display(Name = "Рейтинг")]
        public double Rating { get; set; }

        [Required(ErrorMessage = "Год выпуска обязателен")]
        [Display(Name = "Год выпуска")]
        [Range(1900, 2026, ErrorMessage = "Год выпуска должен быть в диапазоне 1900-2026")]
        public int YearOfRelease { get; set; } = DateTime.Now.Year;

        [Required(ErrorMessage = "Выберите жанр")]
        [Display(Name = "Жанр")]
        public string Genre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Выберите тип контента")]
        [Display(Name = "Тип контента")]
        public string ContentType { get; set; } = "Фильм"; 
    }
    public class Content
    {
        public string Name { get; set; }
        public string Value {  get; set; }
    }
}