using DBComm.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using DBComm.Logic;

namespace DBComm.Repository
{
    public class SaveDataRepository : ISaveDataRepository
    {
        private readonly Context _context;

        public SaveDataRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<TemperatureReading> SaveTemperatureReading(double value, DateTime readAt)
        {
            try
            {
                Room? defaultRoom = await _context.room.FindAsync("1");
                if (defaultRoom == null) throw new Exception("Default room not found.");
                
                TemperatureReading temperatureReading = new TemperatureReading(value, readAt)
                {
                    Room = defaultRoom
                };
                
                await _context.temperature_reading.AddAsync(temperatureReading);
                await _context.SaveChangesAsync();
                return temperatureReading;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}