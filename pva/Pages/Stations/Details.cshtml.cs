using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pva.Models;
using pva.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pva.Pages.Stations
{
    public class DetailsModel : PageModel
    {
        private readonly IStationService _stationService;

        public DetailsModel(IStationService stationService)
        {
            _stationService = stationService;
        }

        public Station Station { get; set; } = default!;
        public IList<StationHistoricalData> HistoricalData { get; set; } = new List<StationHistoricalData>();

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

            HistoricalData = await _stationService.GetHistoricalDataAsync(id.Value);

            return Page();
        }
    }
}
