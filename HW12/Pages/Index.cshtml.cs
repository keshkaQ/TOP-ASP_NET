using HW12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HW12.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<User> Users { get; private set; } = new();
        public string CurrentDatabase { get; set; } = "Postgres";

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet(string database = "Postgres")
        {
            CurrentDatabase = database;
            LoadUsers(database);
            return Page();
        }

        public IActionResult LoadUsers(string database)
        {
            var connectionString = database switch
            {
                "SqlServer" => _configuration.GetConnectionString("SqlServer"),
                "PostgresDocker" => _configuration.GetConnectionString("PostgresDocker"),
                _ => _configuration.GetConnectionString("Postgres") 
            };

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            if (connectionString.StartsWith("Server"))
                optionsBuilder.UseSqlServer(connectionString);
            else
                optionsBuilder.UseNpgsql(connectionString);

            using var tempContext = new AppDbContext(optionsBuilder.Options);

            Users = tempContext.Users.ToList();
            return Page();
        }
        public IActionResult OnPostSwitchDatabase(string database)
        {
            CurrentDatabase = database;
            LoadUsers(database);
            return Page();
        }
    }
}
