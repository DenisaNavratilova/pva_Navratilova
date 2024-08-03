using Microsoft.EntityFrameworkCore;
using pva.Models;

namespace pva.Services
{
    public class MainService
    {
        private readonly ApplicationDbContext _context;

        public MainService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(int totalValues, int totalStations, IList<StationDailyAverage> stationDailyAverages)> GetDashboardDataAsync()
        {
            int totalValues = await _context.Values.CountAsync();
            int totalStations = await _context.Stations.CountAsync();

            DateTime currentDate = DateTime.Now.Date;

            var validValuesForGraph = await _context.Values
                .Include(v => v.Station)
                .Where(value => value.Timestamp.Date == currentDate)
                .ToListAsync();

            var stationDailyAverages = validValuesForGraph
                .GroupBy(v => new { v.Station.Name, v.Timestamp.Date })
                .Select(g => new StationDailyAverage
                {
                    StationName = g.Key.Name,
                    Date = g.Key.Date,
                    AverageLevel = g.Average(v => v.Level)
                })
                .OrderBy(avg => avg.StationName).ThenBy(avg => avg.Date)
                .ToList();

            return (totalValues, totalStations, stationDailyAverages);
        }

        public class StationDailyAverage
        {
            public string StationName { get; set; }
            public DateTime Date { get; set; }
            public double AverageLevel { get; set; }
        }
    }
}
