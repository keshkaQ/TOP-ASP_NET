using Bogus;
using HW12.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HW12.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AppDbContext _dbContext;
        public List<User> Users { get; private set; } = new();

        public IndexModel(AppDbContext dbContext, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public void OnGet()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
            var people = new Faker<User>()
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Address, f => f.Address.FullAddress())
                .Generate(10);
            _dbContext.Users.AddRange(people);
            _dbContext.SaveChanges();
            Users = people;
        }
    }
}
