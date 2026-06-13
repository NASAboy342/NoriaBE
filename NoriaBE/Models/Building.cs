using System;

namespace NoriaBE.Models;

public class Building
{
	public int Id { get; set; } = 0;
	public string Name { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string Img { get; set; } = string.Empty;
	public int Rooms { get; set; } = 0;
	public int OccupiedRooms { get; set; } = 0;
	public int PaidRooms { get; set; } = 0;
    public int Floors { get; set; }
    public decimal ElectricityPricePerUnit { get; set; }
    public decimal WaterPricePerUnit { get; set; }
	public decimal KHRToUSDExchangeRate { get; set; }
}
