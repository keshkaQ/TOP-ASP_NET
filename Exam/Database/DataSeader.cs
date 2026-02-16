using Bogus;
using Exam.Models;

namespace Exam.Database
{
    public class DataSeader
    {
        private readonly AppDbContext _dbContext;

        public DataSeader(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public void SeedData()
        {
            if (!_dbContext.Clients.Any() && !_dbContext.Courts.Any() && !_dbContext.Bookings.Any())
            {
                // Создание кортов
                var courts = new List<TennisCourt>
                {
                    new TennisCourt
                    {
                        Id = Guid.NewGuid(),
                        Name = "Центральный корт",
                        HourlyRate = 2000,
                        Description = "Основной корт с трибунами"
                    },
                    new TennisCourt
                    {
                        Id = Guid.NewGuid(),
                        Name = "Корт 2",
                        HourlyRate = 1500,
                        Description = "Грунтовое покрытие"
                    },
                    new TennisCourt
                    {
                        Id = Guid.NewGuid(),
                        Name = "Корт 3",
                        HourlyRate = 1200,
                        Description = "Грунтовое покрытие"
                    },
                };

                _dbContext.Courts.AddRange(courts);
                _dbContext.SaveChanges();

                // Создание клиентов
                var clientFaker = new Faker<Client>("ru")
                       .RuleFor(c => c.Id, f => Guid.NewGuid())
                       .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                       .RuleFor(c => c.LastName, f => f.Name.LastName())
                       .RuleFor(c => c.Email, f => f.Internet.Email())
                       .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber("+7 (9##) ###-##-##"))
                       .RuleFor(c => c.RegistrationDate, f => f.Date.Past(1).ToUniversalTime());

                var clients = clientFaker.Generate(10);
                _dbContext.Clients.AddRange(clients);
                _dbContext.SaveChanges();

                // Создание бронирований
                var bookingFaker = new Faker<Booking>("ru")
                    .RuleFor(b => b.Id, f => Guid.NewGuid())
                    .RuleFor(b => b.TennisCourtId, f => f.PickRandom(courts).Id)
                    .RuleFor(b => b.ClientId, f => f.PickRandom(clients).Id)
                    .RuleFor(b => b.StartTime, f => f.Date.Soon(30).ToUniversalTime())
                    .RuleFor(b => b.EndTime, (f, b) => b.StartTime.AddHours(f.Random.Int(1, 3)))
                    .RuleFor(b => b.Status, f => f.PickRandom<Status>())
                    .RuleFor(b => b.CreatedAt, f => f.Date.Recent(10).ToUniversalTime())
                    .RuleFor(b => b.TotalCost, (f, b) =>
                    {
                        var court = courts.First(c => c.Id == b.TennisCourtId);
                        var hours = (b.EndTime - b.StartTime).TotalHours;
                        return court.HourlyRate * (decimal)hours;
                    });

                var bookings = bookingFaker.Generate(15);
                _dbContext.Bookings.AddRange(bookings);
                _dbContext.SaveChanges();
            }
        }
    }
}
