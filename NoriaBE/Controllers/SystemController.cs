using System;
using Microsoft.AspNetCore.Mvc;
using NoriaBE.Filters;
using NoriaBE.Models;
using NoriaBE.Services;

namespace NoriaBE.Controllers;

[ApiController]
[Route("[controller]")]
[ServiceFilter(typeof(ExceptionFilter))]
public class SystemController : ControllerBase
{
    private readonly IBuildingService _buildingService;
    private readonly IRoomService _roomService;

    public SystemController(IBuildingService buildingService, IRoomService roomService)
    {
        _buildingService = buildingService;
        _roomService = roomService;
    }

    [HttpGet("get-all-building")]
    public IActionResult GetAllBuilding()
    {
        var buildings = _buildingService.GetAllBuilding();
        var rooms = _roomService.GetAllRoom();
        _buildingService.SumRooms(buildings,rooms);
        return Ok(buildings);
    }
    [HttpGet("get-all-room-of-building")]
    public IActionResult GetAllRoomOfBuilding(int buildingId)
    {
        var rooms = _roomService.GetAllRoomByBuildingId(buildingId);
        return Ok(rooms);
    }
     [HttpPost("add-building")]
    public IActionResult AddBuilding(Building building)
    {
        _buildingService.AddBuilding(building);
        return Ok();
    }
    [HttpPost("add-room")]
    public IActionResult AddRoom(Room room)
    {
        _buildingService.ValidateBuildingId(room.BuildingId);
        _roomService.AddRoom(room);
        return Ok();
    }
    [HttpPost("update-room")]
    public IActionResult UpdateRoom(Room room)
    {

        _buildingService.ValidateBuildingId(room.BuildingId);
        _roomService.UpdateRoom(room);
        return Ok();
    }
    [HttpPost("update-building")]
    public IActionResult UpdateBuilding(Building building)
    {
        _buildingService.UpdateBuilding(building);
        return Ok();
    }
}
