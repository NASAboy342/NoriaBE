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
        existingRoom.Deposit = room.Deposit;
        existingRoom.RequiredDepositAmount = room.RequiredDepositAmount;
        _noriaRepository.UpdateRoom(existingRoom);
    }

    public void ValidateAddRoomRequest(Room room)
    {
        if (string.IsNullOrEmpty(room.Name))
        {
            throw new Exception("Room name is required.");
        }
        if (room.BuildingId <= 0)
        {
            throw new Exception("BuildingId must be greater than 0.");
        }
        if (room.Floor < 0)
        {
            throw new Exception("Floor must be greater than or equal to 0.");
        }
    }

    public void ValidateRoomId(int roomId)
    {
        var rooms = _noriaRepository.GetAllRoom();
        if (!rooms.Any(r => r.Id == roomId))
        {
            throw new Exception($"Room with Id {roomId} does not exist.");
        }
    }
}
