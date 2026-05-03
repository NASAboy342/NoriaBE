using System;
using NoriaBE.Models;

namespace NoriaBE.Repositories;

public interface INoriaRepository
{
    void AddBuilding(Building building);
    void AddRoom(Room room);
    List<Building> GetAllBuilding();
    List<Room> GetAllRoom();
    void UpdateBuilding(Building existingBuilding);
    void UpdateRoom(Room existingRoom);
}
