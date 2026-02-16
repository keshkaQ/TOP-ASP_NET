using Exam.Database;
using Exam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exam.Pages.Clients
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Client> Clients { get; set; } = [];

        public async Task OnGetAsync()
        {
            Clients = await _context.Clients
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Клиент {client.FirstName} {client.LastName} успешно удален";
            }
            else
            {
                TempData["ErrorMessage"] = "Клиент не найден";
            }

            return RedirectToPage();
        }
    }
}