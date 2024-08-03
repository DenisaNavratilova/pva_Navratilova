using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using pva.Services;

namespace pva.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly MainService _mainService;

        public IndexModel(ILogger<IndexModel> logger, MainService mainService)
        {
            _logger = logger;
            _mainService = mainService;
        }

        public int TotalValues { get; set; }
        public int TotalStations { get; set; }

        public IList<MainService.StationDailyAverage> StationDailyAverages { get; set; }

        public async Task OnGetAsync()
        {
            var (totalValues, totalStations, stationDailyAverages) = await _mainService.GetDashboardDataAsync();

            TotalValues = totalValues;
            TotalStations = totalStations;
            StationDailyAverages = stationDailyAverages;
        }
    }
}
