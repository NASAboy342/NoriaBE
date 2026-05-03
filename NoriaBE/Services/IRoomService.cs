using System;
using NoriaBE.Models;
namespace NoriaBE.Services;

public interface IRoomService
{
    void AddRoom(Room room);
    List<Room> GetAllRoom();
    List<Room> GetAllRoomByBuildingId(int buildingId);
    void UpdateRoom(Room room);
}
