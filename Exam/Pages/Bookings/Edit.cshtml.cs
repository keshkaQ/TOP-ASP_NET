using Exam.Database;
using Exam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Exam.Pages.Bookings
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Booking Booking { get; set; } = new Booking();

        public List<SelectListItem> CourtList { get; set; }
        public List<SelectListItem> ClientsList { get; set; }
        public string CourtPricesJson { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            // Загружаем бронирование с связанными данными
            Booking = await _context.Bookings
                .Include(b => b.TennisCourt)
                .Include(b => b.Client)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (Booking == null)
            {
                TempData["ErrorMessage"] = "Бронирование не найдено";
                return RedirectToPage("./Index");
            }

            Booking.StartTime = EnsureUtc(Booking.StartTime);
            Booking.EndTime = EnsureUtc(Booking.EndTime);

            await LoadListsAsync();

            // Загружаем цены кортов для JavaScript
            var courtPrices = await _context.Courts
                .ToDictionaryAsync(c => c.Id.ToString(), c => c.HourlyRate);
            CourtPricesJson = JsonSerializer.Serialize(courtPrices);

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

            ClientsList = await _context.Clients
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.FirstName} {c.LastName}"
                })
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadListsAsync();

            Booking.StartTime = EnsureUtc(Booking.StartTime);
            Booking.EndTime = EnsureUtc(Booking.EndTime);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Booking.StartTime >= Booking.EndTime)
            {
                ModelState.AddModelError("Booking.EndTime", "Время окончания должно быть позже времени начала");
                return Page();
            }

            if (Booking.StartTime < DateTime.UtcNow)
            {
                ModelState.AddModelError("Booking.StartTime", "Дата начала не может быть в прошлом");
                return Page();
            }

            var court = await _context.Courts.FindAsync(Booking.TennisCourtId);
            if (court == null)
            {
                ModelState.AddModelError("Booking.TennisCourtId", "Выбранный корт не найден");
                return Page();
            }

            var hours = (decimal)(Booking.EndTime - Booking.StartTime).TotalHours;
            Booking.TotalCost = hours * court.HourlyRate;

            // Проверка доступности
            var isCourtBusy = await _context.Bookings
                .AnyAsync(b => b.TennisCourtId == Booking.TennisCourtId &&
                              b.Id != Booking.Id &&
                              b.Status != Status.Cancelled &&
                              ((Booking.StartTime >= b.StartTime && Booking.StartTime < b.EndTime) ||
                               (Booking.EndTime > b.StartTime && Booking.EndTime <= b.EndTime) ||
                               (Booking.StartTime <= b.StartTime && Booking.EndTime >= b.EndTime)));

            if (isCourtBusy)
            {
                ModelState.AddModelError("Booking.StartTime", "Этот корт занят в выбранное время другим бронированием");
                return Page();
            }

            try
            {
                var existingBooking = await _context.Bookings.FindAsync(Booking.Id);
                if (existingBooking == null)
                {
                    return NotFound();
                }
                existingBooking.TennisCourtId = Booking.TennisCourtId;
                existingBooking.ClientId = Booking.ClientId;
                existingBooking.StartTime = Booking.StartTime;
                existingBooking.EndTime = Booking.EndTime;
                existingBooking.TotalCost = Booking.TotalCost;
                existingBooking.Status = Booking.Status;

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Бронирование для корта '{court.Name}' успешно обновлено";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Ошибка при сохранении: {ex.Message}");
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