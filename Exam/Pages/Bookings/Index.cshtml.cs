using Exam.Database;
using Exam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exam.Pages.Bookings
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Booking> Bookings { get; set; } = [];

        public async Task OnGetAsync()
        {
            Bookings = await _context.Bookings
                .Include(b => b.Client)
                .Include(b => b.TennisCourt)
                .OrderByDescending(b => b.StartTime)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Броинрование успешно удалено";
            }
            else
            {
                TempData["ErrorMessage"] = "Бронирование не найдено";
            }

            return RedirectToPage();
        }
    }
}