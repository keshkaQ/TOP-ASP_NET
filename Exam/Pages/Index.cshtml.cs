using Exam.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exam.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _dbContext;
        private readonly DataSeader _dataSeader;

        public int ClientsCount { get; set; }
        public int CourtsCount { get; set; }
        public int BookingsCount { get; set; }
     

        public IndexModel(AppDbContext dbContext, DataSeader dataSeader)
        {
            _dbContext = dbContext;
            _dataSeader = dataSeader;
        }

        public IActionResult OnGet(string selectedTable = "clients")
        {
            InitializeDatabase();
            LoadCounts();
            return Page();
        }

        private void InitializeDatabase()
        {
            _dbContext.Database.EnsureCreated();
            _dataSeader.SeedData();
        }

        private void LoadCounts()
        {
            ClientsCount = _dbContext.Clients.Count();
            CourtsCount = _dbContext.Courts.Count();
            BookingsCount = _dbContext.Bookings.Count();
        }
    }
}
