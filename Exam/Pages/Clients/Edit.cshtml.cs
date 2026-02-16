using Exam.Database;
using Exam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exam.Pages.Clients
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Client Client { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            Client = client;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var clientToUpdate = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == Client.Id);

            if (clientToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                _context.Entry(clientToUpdate).CurrentValues.SetValues(Client);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Клиент {Client.FirstName} {Client.LastName} успешно обновлен";
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Clients_Email") == true)
            {
                ModelState.AddModelError("Client.Email", "Клиент с таким email уже существует");
                return Page();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Ошибка при сохранении данных");
                return Page();
            }
        }
    }
}