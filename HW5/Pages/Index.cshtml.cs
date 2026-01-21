using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HW5.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty(SupportsGet = true)]
        [Display(Name = "¬ведите число, факториал которого хотите получить: ")]
        [Range(0, 10, ErrorMessage = "„исло должно быть от 0 до 10")] 
        public int InputNumber { get; set; }

        public int Factorial { get; set; } = 1;
        public string RequestMethod { get; set; } = "";
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(int? number)
        {
            RequestMethod = "GET";
            if(number.HasValue)
            {
                InputNumber = number.Value;
            }
            Factorial = FactorialFunc(InputNumber);
        }
        public IActionResult OnPost() 
        {
            RequestMethod = "POST";
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Factorial = FactorialFunc(InputNumber);
            return Page();
        }

        public static int FactorialFunc(int n) => n == 0 ? 1 : n * FactorialFunc(n - 1);
    }
}
