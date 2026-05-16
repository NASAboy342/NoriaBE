using System;
using Newtonsoft.Json;
using NoriaBE.Models;
using NoriaBE.Repositories;

namespace NoriaBE.Services;

public class BuildingService : IBuildingService
{
    private readonly INoriaRepository _noriaRepository;
    private readonly ILoggerService _loggerService;
    public BuildingService(INoriaRepository noriaRepository, ILoggerService loggerService)
    {
        _noriaRepository = noriaRepository;
        _loggerService = loggerService;
    }
    public List<Building> GetAllBuilding()
    {
        var buildings = _noriaRepository.GetAllBuilding();

        return buildings;
    }
    public void SumRooms(List<Building> buildings, List<Room> rooms)
    {
        foreach (var building in buildings)
        {
            var buildingRooms = rooms.Where(r => r.BuildingId == building.Id).ToList();
            building.Rooms = buildingRooms.Count;
            building.OccupiedRooms = buildingRooms.Count(r => r.IsOccupied);
            building.PaidRooms = buildingRooms.Count(r => r.Price > 0);
        }
    }
    public void AddBuilding(Building building)
    {
        _noriaRepository.AddBuilding(building);
    }

    public void ValidateBuildingId(int buildingId)
    {
        var buildings = _noriaRepository.GetAllBuilding();
        if (!buildings.Any(b => b.Id == buildingId))
        {
            throw new Exception($"Building with Id {buildingId} does not exist.");
        }
    }

    public void UpdateBuilding(Building building)
    {
        var buildings = _noriaRepository.GetAllBuilding();
        var existingBuilding = buildings.FirstOrDefault(b => b.Id == building.Id);
        if (existingBuilding == null)
        {
            throw new Exception($"Building with Id {building.Id} does not exist.");
        }
        _loggerService.Info($"building data before update: {JsonConvert.SerializeObject(existingBuilding)}");
        existingBuilding.Name = building.Name;
        existingBuilding.Address = building.Address;
        existingBuilding.Img = building.Img;
        existingBuilding.Floors = building.Floors;
        existingBuilding.ElectricityPricePerUnit = building.ElectricityPricePerUnit;
        existingBuilding.WaterPricePerUnit = building.WaterPricePerUnit;
        existingBuilding.KHRToUSDExchangeRate = building.KHRToUSDExchangeRate;
        _loggerService.Info($"building data update: {JsonConvert.SerializeObject(existingBuilding)}");
        _noriaRepository.UpdateBuilding(existingBuilding);
        
    }

    public void ValidateAddBuildingRequest(Building building)
    {
        if (string.IsNullOrEmpty(building.Name))
        {
            throw new Exception("Building name is required.");
        }
        if (string.IsNullOrEmpty(building.Address))
        {
            throw new Exception("Building address is required.");
        }
        if (building.Floors <= 0)
        {
            throw new Exception("Building floors must be greater than 0.");
        }
        if (building.ElectricityPricePerUnit <= 100)
        {
            throw new Exception("Building electricity price per unit must be greater than 100 Riels.");
        }
        if (building.WaterPricePerUnit <= 100)
        {
            throw new Exception("Building water price per unit must be greater than 100 Riels.");
        }
    }
}
