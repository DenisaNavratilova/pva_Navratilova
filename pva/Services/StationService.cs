using Microsoft.EntityFrameworkCore;
using pva.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pva.Services
{
    public interface IStationService
    {
        Task CreateStationAsync(Station station);
        Task DeleteStationAsync(int stationId);
        Task<Station> GetStationDetailsAsync(int stationId);
        Task<List<StationHistoricalData>> GetHistoricalDataAsync(int stationId);
        Task<List<Station>> GetAllStationsAsync();
        Task UpdateStationAsync(Station station);
    }

    public class StationService : IStationService
    {
        private readonly ApplicationDbContext _context;

        public StationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateStationAsync(Station station)
        {
            _context.Stations.Add(station);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStationAsync(int stationId)
        {
            var station = await _context.Stations.FindAsync(stationId);
            if (station != null)
            {
                _context.Stations.Remove(station);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Station> GetStationDetailsAsync(int stationId)
        {
            return await _context.Stations
                .FirstOrDefaultAsync(m => m.StationId == stationId);
        }

        public async Task<List<StationHistoricalData>> GetHistoricalDataAsync(int stationId)
        {
            return await _context.Values
                .Where(v => v.StationId == stationId)
                .GroupBy(v => v.Timestamp.Date)
                .Select(g => new StationHistoricalData
                {
                    Date = g.Key,
                    AverageLevel = g.Average(v => v.Level)
                })
                .OrderBy(d => d.Date)
                .ToListAsync();
        }

        public async Task<List<Station>> GetAllStationsAsync()
        {
            return await _context.Stations.ToListAsync();
        }

        public async Task UpdateStationAsync(Station station)
        {
            _context.Attach(station).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }

    public class StationHistoricalData
    {
        public DateTime Date { get; set; }
        public double AverageLevel { get; set; }
    }
}
