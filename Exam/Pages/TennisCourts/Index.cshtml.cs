using Exam.Database;
using Exam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exam.Pages.TennisCourts
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<TennisCourt> TennisCourts { get; set; } = [];

        public async Task OnGetAsync()
        {
            TennisCourts = await _context.Courts
                .OrderBy(c => c.HourlyRate)
                .ToListAsync();
        }
        public async Task<IActionResult> OnPostDelete(Guid id)
        {
            var court = await _context.Courts.FindAsync(id);
            if (court != null)
            {
                _context.Courts.Remove(court);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"{court.Name} успешно удален";
            }
            else
            {
                TempData["ErrorMessage"] = "Клиент не найден";
            }
            return RedirectToPage();
        }
    }
}