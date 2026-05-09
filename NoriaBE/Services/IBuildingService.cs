using System;
using NoriaBE.Models;

namespace NoriaBE.Services;

public interface IBuildingService
{
    void AddBuilding(Building building);
    List<Building> GetAllBuilding();
    void SumRooms(List<Building> buildings, List<Room> rooms);
    void UpdateBuilding(Building building);
    void ValidateAddBuildingRequest(Building building);
    void ValidateBuildingId(int buildingId);
}
