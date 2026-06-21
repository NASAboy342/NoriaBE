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
    private readonly IPaymentService _paymentService;

    public SystemController(IBuildingService buildingService, IRoomService roomService, IPaymentService paymentService)
    {
        _buildingService = buildingService;
        _roomService = roomService;
        _paymentService = paymentService;
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
        _buildingService.ValidateAddBuildingRequest(building);
        _buildingService.AddBuilding(building);
        return Ok();
    }
    [HttpPost("add-room")]
    public IActionResult AddRoom(Room room)
    {
        _roomService.ValidateAddRoomRequest(room);
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

    [HttpGet("get-room-payments")]
    public IActionResult GetRoomPayments(int roomId, int lastN)
    {
        _roomService.ValidateRoomId(roomId);
        var payments = _paymentService.GetRoomPayments(roomId, lastN);
        return Ok(payments);
    }

    [HttpPost("create-payment")]
    public IActionResult CreatePayment(Usage payment)
    {
        payment.ValidateRequest();
        _roomService.ValidateRoomId(payment.RoomId);
        _paymentService.CreatePayment(payment);
        return Ok();
    }

    [HttpPost("update-payment")]
    public IActionResult UpdatePayment(Usage payment)
    {
        payment.ValidateRequest();
        _roomService.ValidateRoomId(payment.RoomId);
        _paymentService.GetRoomPaymentsById(payment.Id);
        _paymentService.Update(payment);
        return Ok();
    }



    [HttpPost("do-payment")]
    public IActionResult DoPayment(Usage payment)
    {
        payment.ValidateRequest();
        _roomService.ValidateRoomId(payment.RoomId);
        var targetPayment = _paymentService.GetRoomPaymentsById(payment.Id);
        _paymentService.DoPayment(targetPayment, payment);
        _paymentService.Update(targetPayment);
        return Ok();
    }

    [HttpGet("get-room-payments-by-id")]   
     public IActionResult GetRoomPaymentsById(int paymentId)
    {
        var payment = _paymentService.GetRoomPaymentsById(paymentId);
        return Ok(payment);
    }
}
