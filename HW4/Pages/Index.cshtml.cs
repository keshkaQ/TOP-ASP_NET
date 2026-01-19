using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HW4.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public double Roubles { get; set; }
        public double UsdInput { get; set; }
        public double Course { get; } = 77.83;
        public bool HasResult { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(double? usdInput)
        {
            if (usdInput.HasValue && usdInput.Value > 0)
            {
                HasResult = true;
                UsdInput = usdInput.Value;
                Roubles = UsdInput * Course;
            }
            else
            {
                HasResult = false;
            }
        }
    }
}
