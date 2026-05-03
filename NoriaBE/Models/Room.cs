using System;

namespace NoriaBE.Models;

public class Room
{
    public int Id { get; set; }
    public int BuildingId { get; set; }
    public bool IsOccupied { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public int Floor { get; set; }
    public decimal Price { get; set; }
}
