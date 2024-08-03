using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pva.Models;
using pva.Services;
using System.Threading.Tasks;

namespace pva.Pages.Stations
{
    public class CreateModel : PageModel
    {
        private readonly IStationService _stationService;

        public CreateModel(IStationService stationService)
        {
            _stationService = stationService;
        }

        [BindProperty]
        public Station Station { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _stationService.CreateStationAsync(Station);

            return RedirectToPage("./Index");
        }
    }
}
