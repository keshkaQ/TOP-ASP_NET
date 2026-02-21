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
        private readonly ILogger<CreateModel> _logger;

        public EditModel(AppDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
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
                _logger.LogError($"Привязка клиента завершилась с ошибкой");
                return Page();
            }

            var clientToUpdate = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == Client.Id);

            if (clientToUpdate == null)
            {
                _logger.LogWarning($"Клиент с именем {Client.FirstName} {Client.LastName} не найден");
                return NotFound();
            }

            try
            {
                _context.Entry(clientToUpdate).CurrentValues.SetValues(Client);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Клиент {Client.FirstName} {Client.LastName} успешно обновлен";
                _logger.LogInformation($"Данные клиента {Client.FirstName} успешно обновлны");
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Clients_Email") == true)
            {
                ModelState.AddModelError("Client.Email", "Клиент с таким email уже существует");
                _logger.LogError($"Клиент с {Client.Email} уже существует");
                return Page();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Ошибка при сохранении данных");
                _logger.LogError($"Ошибка при сохранении клиента с именем {Client.FirstName} {Client.LastName}");
                return Page();
            }
        }
    }
}