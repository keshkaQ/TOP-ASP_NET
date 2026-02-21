using Exam.Database;
using Exam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exam.Pages.Clients
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
        public Client Client { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Привязка клиента завершилась с ошибкой");
                return Page();
            }

            Client.Id = Guid.NewGuid();
            Client.RegistrationDate = DateTime.UtcNow;

            try
            {
                _context.Clients.Add(Client);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Клиент {Client.FirstName} {Client.LastName} успешно добавлен";
                _logger.LogInformation($"Клиент {Client.FirstName} {Client.LastName} успешно добавлен");
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("IX_Clients_Email"))
                {
                    ModelState.AddModelError("Client.Email", "Клиент с таким email уже существует");
                    _logger.LogError($"Клиент с {Client.Email} уже существует");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ошибка при сохранении данных");
                    _logger.LogError($"Ошибка при сохранении клиента с именем {Client.FirstName} {Client.LastName}");
                }

                return Page();
            }
        }
    }
}