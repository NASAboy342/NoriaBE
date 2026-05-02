using System;
using NoriaBE.Models;
using NoriaBE.Repositories;

namespace NoriaBE.Services;

public class BuildingService : IBuildingService
{
    private readonly INoriaRepository _noriaRepository;
    public BuildingService(INoriaRepository noriaRepository)
    {
        _noriaRepository = noriaRepository;
    }
    public List<Building> GetAllBuilding()
    {
        var buildings = _noriaRepository.GetAllBuilding();
        return buildings;
    }
}
