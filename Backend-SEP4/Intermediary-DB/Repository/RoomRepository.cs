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

    public async Task AddRoom(string name, string deviceId, string homeId)
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
        Room? room = await _context.room.FirstOrDefaultAsync(r => r.Id == id);
        if (room != null)
        {
            _context.room.Remove(room);
            await _context.SaveChangesAsync();
        }
    }

    public async Task EditRoom(string id, string? name=null, string? deviceId=null)
    {
        Room room = await _context.room.FirstOrDefaultAsync(r => r.Id == id);
        if (room != null)
        {
            if (name != null)
            {
                room.Name = name;
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

    public async Task<List<Room>> GetAllRooms(string homeId, string? deviceId=null)
    {
        IQueryable<Room> query = _context.room.Include(r=>r.Home).Where(r => r.Home.Id == homeId);

        if (deviceId != null)
        {
            query = query.Where(r => r.DeviceId == deviceId);
        }
        
        List<Room> rooms = await query.ToListAsync();
        
        if (rooms.Count==0)
        {
            throw new Exception($"No room with device {deviceId} or given wrong house ID");
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

    public async Task<bool> CheckNonExistingRoom(string id)
    {
        Room? existing = await _context.room.FirstOrDefaultAsync(r => r.Id == id);
        if (existing==null)
        {
            throw new Exception($"Room {id} doesn't exist in home");
        }
        return true;
    }
}