using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pva.Models;
using pva.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static pva.Pages.IndexModel;
using static pva.Services.MainService;
using static pva.Services.StationService;

namespace pva.Pages
{
    public class ValuesModel : PageModel
    {
        private readonly IValuesService _valuesService;

        public ValuesModel(IValuesService valuesService)
        {
            _valuesService = valuesService;
        }

        public IList<Value> Values { get; set; }
        public IList<Station> Stations { get; set; }
        public IList<StationDailyAverage> StationDailyAverages { get; set; }
        public DateTime? FilterDate { get; set; }
        public int? FilterStationId { get; set; }
        public HashSet<Value> ValuesWithTimeoutWarning { get; set; }
        public bool LevelWarning { get; set; }

        public async Task OnGetAsync(DateTime? filterDate, int? filterStationId)
        {
            FilterDate = filterDate;
            FilterStationId = filterStationId;

            var (values, valuesWithTimeoutWarning, levelWarning) = await _valuesService.GetValuesAsync(filterDate, filterStationId);
            Values = values;
            ValuesWithTimeoutWarning = valuesWithTimeoutWarning;
            LevelWarning = levelWarning;

            Stations = await _valuesService.GetStationsAsync();
            StationDailyAverages = await _valuesService.GetStationDailyAveragesAsync(filterDate);
        }
    }
}
