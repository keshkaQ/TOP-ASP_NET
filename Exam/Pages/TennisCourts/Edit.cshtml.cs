using Exam.Database;
using Exam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exam.Pages.TennisCourts
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EditModel> _logger;
        public EditModel(AppDbContext context, ILogger<EditModel> logger)
        {
            _context = context;
            _logger = logger;
        }
     
        [BindProperty]
        public TennisCourt TennisCourt { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var tennisCourt = await _context.Courts.FindAsync(id);
            if(tennisCourt == null)
            {
                _logger.LogWarning($"Корт с именем {TennisCourt.Name} не найден");
                return NotFound();
            }
            TennisCourt = tennisCourt;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                _logger.LogError($"Привязка завершилась с ошибкой");
                return Page();
            }
            var courtToUpdate = await _context.Courts.FirstOrDefaultAsync
                (c => c.Id == TennisCourt.Id);
            if (courtToUpdate == null)
            {
                _logger.LogWarning($"Корт с именем {TennisCourt.Name} не найден");
                return NotFound();
            }

            try
            {
                _context.Entry(courtToUpdate).CurrentValues.SetValues(TennisCourt);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"{TennisCourt.Name} успешно обновлен";
                _logger.LogInformation($"{TennisCourt.Name} успешно обновлен");
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Courts_Name") == true)
            {
                ModelState.AddModelError("TennisCourt.Name", "Корт с таким именем уже существует");
                _logger.LogError($"Корт с {TennisCourt.Name} именем уже существует");
                return Page();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Ошибка при сохранении данных");
                _logger.LogError($"Ошибка при сохранении корта с именем {TennisCourt.Name}");
                return Page();
            }
        }
    }
}
