using System;
using NoriaBE.Models;
using NoriaBE.Repositories;

namespace NoriaBE.Services;

public class RoomService : IRoomService
{
    private readonly INoriaRepository _noriaRepository;

    public RoomService(INoriaRepository noriaRepository)
    {
        _noriaRepository = noriaRepository;
    }

    public void AddRoom(Room room)
    {
        _noriaRepository.AddRoom(room);
    }

    public List<Room> GetAllRoom()
    {
        return _noriaRepository.GetAllRoom();
    }

    public List<Room> GetAllRoomByBuildingId(int buildingId)
    {
        return _noriaRepository.GetAllRoom().Where(r => r.BuildingId == buildingId).ToList();
    }

    public void UpdateRoom(Room room)
    {

        var rooms = _noriaRepository.GetAllRoom();
        var existingRoom = rooms.FirstOrDefault(r => r.Id == room.Id);
        if (existingRoom == null)
        {
            throw new Exception($"Room with Id {room.Id} does not exist.");
        }
        existingRoom.BuildingId = room.BuildingId;
        existingRoom.IsOccupied = room.IsOccupied;
        existingRoom.PhoneNumber = room.PhoneNumber;
        existingRoom.Floor = room.Floor;
        existingRoom.Price = room.Price;
        _noriaRepository.UpdateRoom(existingRoom);
    }
}
