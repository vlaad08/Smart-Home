using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;
using WebAPI.DTOs;

namespace DBComm.Logic;

public class HumidityLogic : IHumidityLogic
{
    private IHumidityRepository _repository;
    private IRoomRepository _roomRepository;
    private INotificationRepository _notificationRepository;
    public HumidityLogic(IHumidityRepository repository, IRoomRepository roomRepository, INotificationRepository notificationRepository)
    {
        _repository = repository;
        _roomRepository = roomRepository;
        _notificationRepository = notificationRepository;
    }
    
    public async Task<HumidityReading> GetLatestHumidity(string hardwareId)
    {
        return await _repository.GetLatestHumidity(hardwareId);
    }
    public async Task<ICollection<HumidityReading>> GetHumidityHistory(string hardwareId, DateTime dateFrom, DateTime dateTo)
    {
        return await _repository.GetHistory(hardwareId, dateFrom, dateTo);
    }

    public async Task SaveHumidityReading(string deviceId, double value)
    {
        RoomDataDTO dto = await _roomRepository.GetRoomData(null, deviceId);
        if (dto == null)
        {
            throw new InvalidOperationException("Room data is null.");
        }
        if (dto.Home == null)
        {
            throw new InvalidOperationException("Home data is null.");
        }
        if ((double)dto.PreferedHumidity < value)
        {
            List<Notification> notifications = await _notificationRepository.GetNotifications(dto.Home.Id);

            if (notifications == null || notifications.Count == 0)
            {
                await _notificationRepository.AddNotification(dto.Home.Id,
                    "The humidity in " + dto.Name + " is higher than preferred and it is " + value + "%");
            }
            else
            {
                bool exists = notifications.Any(n => n.Message.StartsWith("The humidity in " + dto.Name + " is higher than preferred"));
                Console.WriteLine(exists);
                if (!exists)
                {
                    await _notificationRepository.AddNotification(dto.Home.Id,
                        "The humidity in " + dto.Name + " is higher than preferred and it is " + value + "%");
                }
            }
        }

        DateTime dateTime = DateTime.UtcNow.AddHours(2);
        await _repository.SaveHumidityReading(deviceId, value, dateTime);
    }
}