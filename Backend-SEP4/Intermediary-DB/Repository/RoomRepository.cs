using DBComm.Logic;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebAPI.DTOs;

namespace DBComm.Repository;

public class RoomRepository : IRoomRepository
{
    private Context _context;

    public RoomRepository(Context context)
    {
        _context = context;
    }

    public async Task AddRoom(string name, string deviceId, string homeId, int preferedTemperature, int preferedHumidity)
    {
        try
        {
            Home? home = await _context.home.FindAsync(homeId);
            if (home == null)
            {
                throw new Exception("Home doesn't exist");
            }
            Room room = new Room(name, deviceId);
            room.Home = home;
            room.PreferedTemperature = preferedTemperature;
            room.PreferedHumidity = preferedHumidity;
            EntityEntry<Room> r = await _context.room.AddAsync(room);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public async Task DeleteRoom(string id)
    {
        Room? room = await _context.room.FirstOrDefaultAsync(r => r.DeviceId == id);
        if (room != null)
        {
            _context.room.Remove(room);
            await _context.SaveChangesAsync();
        }
    }

    public async Task EditRoom(string id,string? name, string? deviceId, int? preferedTemperature, int? preferedHumidity)
    {
        Room room = await _context.room.FirstOrDefaultAsync(r => r.Id == id);
        if (room != null)
        {
            if (name != null && preferedTemperature != null && preferedHumidity != null)
            {
                room.Name = name;
                room.PreferedTemperature = preferedTemperature;
                room.PreferedHumidity = preferedHumidity;
            }

            if (deviceId != null)
            {
                int sameRoomCount =  _context.room.Count(r => r.DeviceId == deviceId);
                if (sameRoomCount > 1)
                {
                    throw new Exception("One device can only be assigned to one room!");
                }
                room.DeviceId = deviceId;
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<Room>> GetAllRooms(string homeId)
    {
        IQueryable<Room> query = _context.room.Include(r=>r.Home).Where(r => r.Home.Id == homeId);
        
        List<Room> rooms = await query.ToListAsync();
        
        if (rooms.Count==0)
        {
            throw new Exception($"No room were found.");
        }
        
        return rooms;
    }

    public async Task<RoomDataTransferDTO> GetRoomData(string homeId, string deviceId, bool temp = false,
        bool humi = false, bool light = false)
    {
        RoomDataTransferDTO dto = new RoomDataTransferDTO();
        Room? room = await _context.room.FirstOrDefaultAsync(r => r.DeviceId == deviceId && r.Home.Id == homeId);
        if (room != null)
        {
            string roomId = room.Id;
            if (temp)
            {
                var tempReading = _context.temperature_reading.Where(t => t.Room.Id == roomId).OrderByDescending(t => t.ReadAt);
                double? value;
                try
                {
                     value = tempReading.First().Value;
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e);
                    value = null;
                }
                
                dto.TempValue = value;
            }
            if (humi)
            {
                var humiReading = _context.humidity_reading.Where(h => h.Room.Id == roomId).OrderByDescending(h => h.ReadAt);
                double? value;
                try
                {
                    value = humiReading.First().Value;
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e);
                    value = null;
                }
                dto.HumiValue = value;
            }
            if (light)
            {
                var lightReading = _context.light_reading.Where(l => l.Room.Id == roomId).OrderByDescending(l => l.ReadAt);
                double? value;
                try
                {
                    value = lightReading.First().Value;
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e);
                    value = null;
                }
                dto.LightValue = value;
            }

            return dto;
        }
        else
        {
            throw new Exception($"No room with device {deviceId}");
        }
    }



    public async Task<bool> CheckExistingRoom(string deviceId,string homeId)
    {
        Room? existing = await _context.room.FirstOrDefaultAsync(r => r.DeviceId == deviceId);
        if (existing!=null)
        {
            throw new Exception($"Room {deviceId} already exists in home {homeId}");
        }
        return true;
    }

    public async Task<bool> CheckNonExistingRoom(string deviceId)
    {
        Room? existing = await _context.room.FirstOrDefaultAsync(r => r.DeviceId == deviceId);
        if (existing==null)
        {
            throw new Exception($"Room with device {deviceId} doesn't exist in home");
        }
        return true;
    }

    public async Task SetRadiatorLevel(string deviceId, int level)
    {
        Room? existing = await _context.room.FirstOrDefaultAsync(r => r.DeviceId == deviceId);
        if (existing!=null)
        {
            existing.RadiatorState = level;
            _context.room.Update(existing);
            await _context.SaveChangesAsync();
        }
        throw new Exception($"Room  doesn't exist in home");
      
    }

    public async Task<int> GetRadiatorLevel(string deviceId)
    {
        Room? existing = await _context.room.FirstOrDefaultAsync(r => r.DeviceId == deviceId);
        if (existing!=null)
        {
            return existing.RadiatorState;
        }
        throw new Exception($"Room  doesn't exist in home");
    }

    public async Task SaveWindowState(string hardwareId, bool state)
    {
        Room? room = await _context.room.FirstOrDefaultAsync(r =>r.DeviceId.Equals(hardwareId)) ;
        if (room != null)
        {
            if (room.IsWindowOpen && !state) room.IsWindowOpen = false;
            if(!room.IsWindowOpen && state) room.IsWindowOpen = true;
            _context.room.Update(room);
            await _context.SaveChangesAsync();
        }
        throw new Exception($"Room does not exist.");
    }

    public async Task<bool> GetWindowState(string hardwareId)
    {
        Room? room = await _context.room.FirstOrDefaultAsync(r =>r.DeviceId.Equals(hardwareId)) ;
        if (room != null)
        {
            return room.IsWindowOpen;
        }
        throw new Exception($"Room does not exist.");
    }

    public async Task SetLightState(string hardwareId, int level)
    {
        Room? existing = await _context.room.FirstOrDefaultAsync(r => r.DeviceId == hardwareId);
        if (existing!=null)
        {
            existing.LightLevel = level;
            _context.room.Update(existing);
            await _context.SaveChangesAsync();
        }
        throw new Exception($"Room  doesn't exist in home");
    }

    public async Task<int> GetLightState(string hardwareId)
    {
        Room? existing = await _context.room.FirstOrDefaultAsync(r => r.DeviceId == hardwareId);
        if (existing!=null)
        {
            return existing.LightLevel;
        }
        throw new Exception($"Room  doesn't exist in home");
    }
}