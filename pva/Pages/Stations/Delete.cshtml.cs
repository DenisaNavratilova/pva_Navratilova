using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pva.Models;
using pva.Services;
using System.Threading.Tasks;

namespace pva.Pages.Stations
{
    public class DeleteModel : PageModel
    {
        private readonly IStationService _stationService;

        public DeleteModel(IStationService stationService)
        {
            _stationService = stationService;
        }

        [BindProperty]
        public Station Station { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Station = await _stationService.GetStationDetailsAsync(id.Value);

            if (Station == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _stationService.DeleteStationAsync(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
