using Exam.Database;
using Exam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Exam.Pages.Bookings
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
        public Booking Booking { get; set; } = new Booking();

        public List<SelectListItem> CourtList { get; set; }
        public List<SelectListItem> ClientsList { get; set; }

        // Для передачи цен в JavaScript
        public string CourtPricesJson { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadListsAsync();

            var courtPrices = await _context.Courts
                .ToDictionaryAsync(
                    c => c.Id.ToString(),
                    c => c.HourlyRate
                );

            // Сериализуем в JSON для передачи в представление
            CourtPricesJson = JsonSerializer.Serialize(courtPrices);

            // Устанавливаем значения по умолчанию
            Booking.StartTime = DateTime.Now.AddDays(1).Date.AddHours(10);
            Booking.EndTime = Booking.StartTime.AddHours(2);
            Booking.Status = Status.Booked;

            return Page();
        }

        private async Task LoadListsAsync()
        {
            CourtList = await _context.Courts
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();
            if (CourtList.Count <= 0)
            {
                _logger.LogError($"Корты не найдены");
            }

            ClientsList = await _context.Clients
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.FirstName} {c.LastName}"
                })
                .ToListAsync();
            if (ClientsList.Count <= 0)
            {
                _logger.LogError($"Клиенты не найдены");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadListsAsync();

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Привязка бронирования завершилась с ошибкой");
                return Page();
            }

            // Конвертируем в UTC
            Booking.StartTime = EnsureUtc(Booking.StartTime);
            Booking.EndTime = EnsureUtc(Booking.EndTime);

            if (Booking.StartTime >= Booking.EndTime)
            {
                ModelState.AddModelError("Booking.EndTime", "Время окончания должно быть позже времени начала");
                _logger.LogError("Время окончания должно быть позже времени начала");
                return Page();
            }

            if (Booking.StartTime < DateTime.UtcNow) 
            {
                ModelState.AddModelError("Booking.StartTime", "Дата начала не может быть в прошлом");
                _logger.LogError("Дата начала не может быть в прошлом");
                return Page();
            }

            var court = await _context.Courts.FindAsync(Booking.TennisCourtId);
            if (court == null)
            {
                ModelState.AddModelError("Booking.TennisCourtId", "Выбранный корт не найден");
                _logger.LogError("Выбранный корт не найден");
                return Page();
            }

            var hours = (decimal)(Booking.EndTime - Booking.StartTime).TotalHours;
            Booking.TotalCost = hours * court.HourlyRate;

            // Проверка доступности корта
            var isCourtBusy = await _context.Bookings
                .AnyAsync(b => b.TennisCourtId == Booking.TennisCourtId &&
                              b.Id != Booking.Id &&
                              b.Status != Status.Cancelled &&
                              ((Booking.StartTime >= b.StartTime && Booking.StartTime < b.EndTime) ||
                               (Booking.EndTime > b.StartTime && Booking.EndTime <= b.EndTime) ||
                               (Booking.StartTime <= b.StartTime && Booking.EndTime >= b.EndTime)));

            if (isCourtBusy)
            {
                ModelState.AddModelError("Booking.StartTime", "Корт занят в выбранное время");
                _logger.LogError("Корт занят в выбранное время");
                return Page();
            }

            Booking.Id = Guid.NewGuid();
            Booking.CreatedAt = DateTime.UtcNow; 

            try
            {
                _context.Bookings.Add(Booking);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Бронирование для корта '{court.Name}' успешно создано. Стоимость: {Booking.TotalCost:C}";
                _logger.LogInformation($"Бронирование для корта '{court.Name}' успешно создано. Стоимость: {Booking.TotalCost:C}");
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Ошибка при сохранении: {ex.Message}");
                _logger.LogError($"Ошибка при бронировании");
                return Page();
            }
        }

        private DateTime EnsureUtc(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }
            else if (dateTime.Kind == DateTimeKind.Local)
            {
                return dateTime.ToUniversalTime();
            }
            return dateTime;
        }
    }
}