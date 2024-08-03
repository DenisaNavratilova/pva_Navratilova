using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pva.Models;
using pva.Services;

namespace pva.Pages.Stations
{
    public class IndexModel : PageModel
    {
        private readonly IStationService _stationService;

        public IndexModel(IStationService stationService)
        {
            _stationService = stationService;
        }

        public IList<Station> Station { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Station = await _stationService.GetAllStationsAsync();
        }
    }
}
