using System;
using Newtonsoft.Json;

namespace NoriaBE.Models;

public class Usage
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public decimal WaterUsage { get; set; }
    public decimal ElectricityUsage { get; set; }
    public decimal WaterPrice { get; set; }
    public decimal ElectricityPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    [JsonProperty("totalAmountToPay")] public decimal TotalAmmountToPay { get; set; }
    [JsonProperty("totalAmountPaid")] public decimal TotalAmmountPaid { get; set; }
    public bool IsPaid { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime PaidOn { get; set; }

    public void ValidateRequest()
    {
        if (RoomId <= 0)
        {
            throw new Exception("RoomId must be greater than 0.");
        }
        if (WaterUsage < 0)
        {
            throw new Exception("WaterUsage must be greater than or equal to 0.");
        }
        if (ElectricityUsage < 0)
        {
            throw new Exception("ElectricityUsage must be greater than or equal to 0.");
        }
        if (WaterPrice < 0)
        {
            throw new Exception("WaterPrice must be greater than or equal to 0.");
        }
        if (ElectricityPrice < 0)
        {
            throw new Exception("ElectricityPrice must be greater than or equal to 0.");
        }
        if (StartTime >= EndTime)
        {
            throw new Exception("StartTime must be earlier than EndTime.");
        }
    }
}
