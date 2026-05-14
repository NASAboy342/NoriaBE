using System;
using NoriaBE.Models;

namespace NoriaBE.Repositories;

public interface INoriaRepository
{
    void AddBuilding(Building building);
    void AddRoom(Room room);
    void CreatePayment(Usage payment);
    List<Building> GetAllBuilding();
    List<Room> GetAllRoom();
    List<Usage> GetRoomPayments(int roomId, DateTime startTime, DateTime endTime);
    List<Usage> GetRoomPayments(int roomId, int lastN);
    Usage GetRoomPaymentsById(int id);
    void UpdateBuilding(Building existingBuilding);
    void UpdatePayment(Usage updatedPayment);
    void UpdateRoom(Room existingRoom);
}
