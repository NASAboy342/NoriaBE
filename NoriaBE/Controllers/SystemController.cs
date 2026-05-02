using System;
using Microsoft.AspNetCore.Mvc;
using NoriaBE.Filters;
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
        return Ok(buildings);
    }
}
