using Exam.Database;
using Exam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exam.Pages.TennisCourts
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        public CreateModel(AppDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public TennisCourt TennisCourt { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Привязка корта завершилась с ошибкой");
                return Page();
            }    


            TennisCourt.Id = new Guid();
            try
            {
                _context.Courts.Add(TennisCourt);
                await _context.SaveChangesAsync();
               
                TempData["SuccessMessage"] = $"{TennisCourt.Name} успешно добавлен";
                _logger.LogInformation($"{TennisCourt.Name} успешно добавлен");
                return RedirectToPage("./Index");
            }
            catch(DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("IX_Clients_Email"))
                {
                    ModelState.AddModelError("TennisCourt.Name", "Корт с таким именем уже существует");
                    _logger.LogError($"Корт с {TennisCourt.Name} именем уже существует");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ошибка при сохранении данных");
                    _logger.LogError($"Ошибка при сохранении корта с именем {TennisCourt.Name}");
                }
                return Page();
            }
            
        }
    }
}
