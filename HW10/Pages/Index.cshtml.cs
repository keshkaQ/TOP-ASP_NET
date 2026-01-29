using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HW10.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
    public class TimeTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var content = await output.GetChildContentAsync();
            var timeContent = content.GetContent();

            if (DateTime.TryParse(timeContent, out var localTime))
            {
                var timeZone = TimeZoneInfo.Local;
                var utcOffset = timeZone.GetUtcOffset(localTime); 
                var offsetHours = utcOffset.TotalHours;

                var sign = offsetHours >= 0 ? "+" : "";

                var newTime = $"Разница с GMT для {localTime}: {sign}{offsetHours}ч";
                output.Content.SetContent(newTime);
            }
            else
            {
                output.Content.SetContent("Неверный формат времени");
            }
        }
    }
}
