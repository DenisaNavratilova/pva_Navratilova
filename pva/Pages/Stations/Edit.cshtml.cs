using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pva.Models;
using pva.Services;

namespace pva.Pages.Stations
{
    public class EditModel : PageModel
    {
        private readonly IStationService _stationService;

        public EditModel(IStationService stationService)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _stationService.UpdateStationAsync(Station);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await StationExists(Station.StationId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> StationExists(int id)
        {
            var station = await _stationService.GetStationDetailsAsync(id);
            return station != null;
        }
    }
}
