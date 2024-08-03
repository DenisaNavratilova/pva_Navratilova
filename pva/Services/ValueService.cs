using pva.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static pva.Pages.IndexModel;
using static pva.Services.StationService;
using static pva.Services.MainService;

namespace pva.Services
{
    public interface IValuesService
    {
        Task<(IList<Value> values, HashSet<Value> valuesWithTimeoutWarning, bool levelWarning)> GetValuesAsync(DateTime? filterDate, int? filterStationId);
        Task<IList<Station>> GetStationsAsync();
        Task<IList<StationDailyAverage>> GetStationDailyAveragesAsync(DateTime? filterDate);
    }

    public class ValuesService : IValuesService
    {
        private readonly ApplicationDbContext _context;

        public ValuesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IList<Value> values, HashSet<Value> valuesWithTimeoutWarning, bool levelWarning)> GetValuesAsync(DateTime? filterDate, int? filterStationId)
        {
            DateTime currentDateTime = DateTime.Now;
            var query = _context.Values
                .Include(v => v.Station)
                .AsQueryable();

            if (filterDate.HasValue)
            {
                query = query.Where(value => value.Timestamp.Date == filterDate.Value.Date);
            }

            if (filterStationId.HasValue)
            {
                query = query.Where(value => value.StationId == filterStationId.Value);
            }

            var values = await query
                .OrderByDescending(v => v.Timestamp)
                .ToListAsync();

            var levelWarning = values.Any(value =>
                value.Level >= value.Station.FloodLevel ||
                value.Level <= value.Station.DroughtLevel);

            var groupedByStation = values
                .GroupBy(v => v.StationId);

            var latestValues = groupedByStation
                .Select(g => g.OrderByDescending(v => v.Timestamp).FirstOrDefault())
                .ToList();

            var timeoutWarnings = latestValues
                .Select(v => new
                {
                    Value = v,
                    IsTimeoutExceeded = v != null &&
                    (currentDateTime - v.Timestamp).TotalMinutes > v.Station.TimeOutinMinutes
                })
                .ToDictionary(x => x.Value, x => x.IsTimeoutExceeded);

            var valuesWithTimeoutWarning = values
                .Where(v => timeoutWarnings.ContainsKey(v) && timeoutWarnings[v])
                .ToHashSet();

            return (values, valuesWithTimeoutWarning, levelWarning);
        }

        public async Task<IList<Station>> GetStationsAsync()
        {
            return await _context.Stations.ToListAsync();
        }

        public async Task<IList<StationDailyAverage>> GetStationDailyAveragesAsync(DateTime? filterDate)
        {
            DateTime targetDate = filterDate ?? DateTime.Now.Date;

            var validValuesForGraph = await _context.Values
                .Include(v => v.Station)
                .Where(value => value.Timestamp.Date == targetDate)
                .ToListAsync();

            return validValuesForGraph
                .GroupBy(v => new { v.Station.Name, v.Timestamp.Date })
                .Select(g => new StationDailyAverage
                {
                    StationName = g.Key.Name,
                    Date = g.Key.Date,
                    AverageLevel = g.Average(v => v.Level)
                })
                .OrderBy(avg => avg.StationName).ThenBy(avg => avg.Date)
                .ToList();
        }
    }
}
