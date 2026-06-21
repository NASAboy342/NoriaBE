using System;
using NoriaBE.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

namespace NoriaBE.Repositories;

public class NoriaRepository : INoriaRepository
{
    private readonly string _connectionString;
    public NoriaRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetSection("ConnectionString").Value ?? string.Empty;
    }

    protected IEnumerable<T> Query<T>(string spName, object parameters = null, string database = "GameProviderDB", int timeoutTime = 180)
    {
        var cnnString = _connectionString;
        using var conn = new SqlConnection(cnnString);
        var sql = conn.Query<T>(spName, parameters, null, true, timeoutTime, CommandType.StoredProcedure);
        return sql;
    }
    protected IEnumerable<T> QueryText<T>(string sqlText, object parameters = null, string database = "GameProviderDB", int timeoutTime = 180)
    {
        var cnnString = _connectionString;
        using var conn = new SqlConnection(cnnString);
        var sql = conn.Query<T>(sqlText, parameters, null, true, timeoutTime, CommandType.Text);
        return sql;
    }

    protected async Task<IEnumerable<T>> QueryAsync<T>(string spName, object parameters = null, string database = "GameProviderDB",int timeoutTime = 180)
    {
        var cnnString = _connectionString;
        using var conn = new SqlConnection(cnnString);
        var sql = await conn.QueryAsync<T>(spName, parameters, commandTimeout: timeoutTime, commandType: CommandType.StoredProcedure);
        return sql;
    }

    protected async Task<IEnumerable<T>> QueryTextAsync<T>(string sqlText, string database = "GameProviderDB", object parameters = null, int timeoutTime = 180)
    {
        var cnnString = _connectionString;
        await using var conn = new SqlConnection(cnnString);
        var sql = await conn.QueryAsync<T>(sqlText, parameters, commandTimeout: timeoutTime, commandType: CommandType.Text);
        return sql;
    }

    protected Tuple<IEnumerable<Result1>, IEnumerable<Result2>> QueryMultiple<Result1, Result2>(
        string spName, object parameters = null, string database = "GameProviderDB")
    {
        var cnnString = _connectionString;
        using var conn = new SqlConnection(cnnString);
        var sql = conn.QueryMultiple(spName, parameters, null, 180, CommandType.StoredProcedure);
        return new Tuple<IEnumerable<Result1>, IEnumerable<Result2>>(sql.Read<Result1>(), sql.Read<Result2>());
    }
    public List<Building> GetAllBuilding()
    {
        var sql = @"select 
                     Id,
                     Name,
                     Address,
                     Img,
                     Floors,
                     ElectricityPricePerUnit,
                     WaterPricePerUnit,
                     KHRToUSDExchangeRate
                    from building WITH (NOLOCK)";
        var result = QueryText<Building>(sql);
        return result.ToList();
    }
    public List<Room> GetAllRoom()
    {
        var sql = @"select 
                     Id,
                     Name,
                     IsOccupied,
                     PhoneNumber,
                     Floor,
                     Price,    
                     BuildingId,
                     Deposit,
                     RequiredDepositAmount
                    from room WITH (NOLOCK)";
        var result = QueryText<Room>(sql);
        return result.ToList();
    }

    public void AddBuilding(Building building)
    {
        var sql = @"INSERT INTO building (Name, Address, Img, Floors, ElectricityPricePerUnit, WaterPricePerUnit, KHRToUSDExchangeRate) 
                    VALUES (@Name, @Address, @Img, @Floors, @ElectricityPricePerUnit, @WaterPricePerUnit, @KHRToUSDExchangeRate)";
        var result = QueryText<Building>(sql, new
        {
            Name = building.Name,
            Address = building.Address,
            Img = building.Img,
            Floors = building.Floors,
            ElectricityPricePerUnit = building.ElectricityPricePerUnit,
            WaterPricePerUnit = building.WaterPricePerUnit,
            KHRToUSDExchangeRate = building.KHRToUSDExchangeRate
        });
    }
    public void AddRoom(Room room)
    {
        var sql = @"INSERT INTO room (Name, BuildingId, IsOccupied, PhoneNumber, Floor, Price, Deposit, RequiredDepositAmount) 
                    VALUES (@Name, @BuildingId, @IsOccupied, @PhoneNumber, @Floor, @Price, @Deposit, @RequiredDepositAmount)";
        var result = QueryText<Room>(sql, new
        {
            Name = room.Name,
            BuildingId = room.BuildingId,
            IsOccupied = room.IsOccupied,
            PhoneNumber = room.PhoneNumber,
            Floor = room.Floor,
            Price = room.Price,
            Deposit = room.Deposit,
            RequiredDepositAmount = room.RequiredDepositAmount
        });
    }
    public void UpdateRoom(Room room)
    {
        var sql = @"UPDATE room 
                    SET Name = @Name, BuildingId = @BuildingId, IsOccupied = @IsOccupied, PhoneNumber = @PhoneNumber, Floor = @Floor, Price = @Price, Deposit = @Deposit, RequiredDepositAmount = @RequiredDepositAmount
                    WHERE Id = @Id";
        var result = QueryText<Room>(sql, new
        {
            Id = room.Id,
            Name = room.Name,
            BuildingId = room.BuildingId,
            IsOccupied = room.IsOccupied,
            PhoneNumber = room.PhoneNumber,
            Floor = room.Floor,
            Price = room.Price,
            Deposit = room.Deposit,
            RequiredDepositAmount = room.RequiredDepositAmount
        });
    }
    public void UpdateBuilding(Building building)
    {
        var sql = @"UPDATE building 
                    SET Name = @Name, Address = @Address, Img = @Img, Floors = @Floors, ElectricityPricePerUnit = @ElectricityPricePerUnit, WaterPricePerUnit = @WaterPricePerUnit, KHRToUSDExchangeRate = @KHRToUSDExchangeRate
                    WHERE Id = @Id";
        var result = QueryText<Building>(sql, new
        {
            Id = building.Id,
            Name = building.Name,
            Address = building.Address,
            Img = building.Img,
            Floors = building.Floors,
            ElectricityPricePerUnit = building.ElectricityPricePerUnit,
            WaterPricePerUnit = building.WaterPricePerUnit,
            KHRToUSDExchangeRate = building.KHRToUSDExchangeRate
        });
    }

    public List<Usage> GetRoomPayments(int roomId, DateTime startTime, DateTime endTime)
    {
        var sql = @"SELECT
                        Id,
                        RoomId,
                        WaterUsage,
                        ElectricityUsage,
                        WaterPrice,
                        ElectricityPrice,
                        StartTime,
                        EndTime,
                        AdjustmentAmount,
                        TotalAmmountToPay,
                        TotalAmmountPaid,
                        IsPaid,
                        CreatedOn,
                        UpdatedOn,
                        PaidOn
                    FROM Usage WITH (NOLOCK)
                    WHERE RoomId = @RoomId
                      AND StartTime >= @StartTime
                      AND EndTime <= @EndTime";
        var result = QueryText<Usage>(sql, new
        {
            RoomId = roomId,
            StartTime = startTime,
            EndTime = endTime
        });
        return result.ToList();
    }

    public void CreatePayment(Usage payment)
    {
        var sql = @"INSERT INTO Usage (RoomId, WaterUsage, ElectricityUsage, WaterPrice, ElectricityPrice, StartTime, EndTime, AdjustmentAmount, TotalAmmountToPay, TotalAmmountPaid, IsPaid, CreatedOn, UpdatedOn, PaidOn) 
                    VALUES (@RoomId, @WaterUsage, @ElectricityUsage, @WaterPrice, @ElectricityPrice, @StartTime, @EndTime, @AdjustmentAmount, @TotalAmmountToPay, @TotalAmmountPaid, @IsPaid, @CreatedOn, @UpdatedOn, @PaidOn)";
        var result = QueryText<Usage>(sql, new
        {
            RoomId = payment.RoomId,
            WaterUsage = payment.WaterUsage,
            ElectricityUsage = payment.ElectricityUsage,
            WaterPrice = payment.WaterPrice,
            ElectricityPrice = payment.ElectricityPrice,
            StartTime = payment.StartTime,
            EndTime = payment.EndTime,
            AdjustmentAmount = payment.AdjustmentAmount,
            TotalAmmountToPay = payment.TotalAmmountToPay,
            TotalAmmountPaid = payment.TotalAmmountPaid,
            IsPaid = payment.IsPaid,
            CreatedOn = payment.CreatedOn,
            UpdatedOn = payment.UpdatedOn,
            PaidOn = payment.PaidOn
        });
    }

    public Usage GetRoomPaymentsById(int id)
    {
        var sql = @"SELECT
                        Id,
                        RoomId,
                        WaterUsage,
                        ElectricityUsage,
                        WaterPrice,
                        ElectricityPrice,
                        StartTime,
                        EndTime,
                        AdjustmentAmount,
                        TotalAmmountToPay,
                        TotalAmmountPaid,
                        IsPaid,
                        CreatedOn,
                        UpdatedOn,
                        PaidOn
                    FROM Usage WITH (NOLOCK)
                    WHERE Id = @Id";
        var result = QueryText<Usage>(sql, new
        {
            Id = id
        });
        return result.FirstOrDefault();
    }

    public void UpdatePayment(Usage updatedPayment)
    {
        var sql = @"UPDATE Usage 
                    SET 
                    RoomId = @RoomId, 
                    WaterUsage = @WaterUsage, 
                    ElectricityUsage = @ElectricityUsage, 
                    WaterPrice = @WaterPrice, 
                    ElectricityPrice = @ElectricityPrice, 
                    StartTime = @StartTime, 
                    EndTime = @EndTime, 
                    AdjustmentAmount = @AdjustmentAmount, 
                    TotalAmmountToPay = @TotalAmmountToPay, 
                    TotalAmmountPaid = @TotalAmmountPaid, 
                    IsPaid = @IsPaid, 
                    UpdatedOn = @UpdatedOn, 
                    PaidOn = @PaidOn
                    WHERE Id = @Id";
        var result = QueryText<Usage>(sql, new
        {
            Id = updatedPayment.Id,
            RoomId = updatedPayment.RoomId,
            WaterUsage = updatedPayment.WaterUsage,
            ElectricityUsage = updatedPayment.ElectricityUsage,
            WaterPrice = updatedPayment.WaterPrice,
            ElectricityPrice = updatedPayment.ElectricityPrice,
            StartTime = updatedPayment.StartTime,
            EndTime = updatedPayment.EndTime,
            AdjustmentAmount = updatedPayment.AdjustmentAmount,
            TotalAmmountToPay = updatedPayment.TotalAmmountToPay,
            TotalAmmountPaid = updatedPayment.TotalAmmountPaid,
            IsPaid = updatedPayment.IsPaid,
            UpdatedOn = DateTime.Now,
            PaidOn = updatedPayment.PaidOn
        });
    }

    public List<Usage> GetRoomPayments(int roomId, int lastN)
    {
        var sql = @"SELECT TOP (@LastN)
                        Id,
                        RoomId,
                        WaterUsage,
                        ElectricityUsage,
                        WaterPrice,
                        ElectricityPrice,
                        StartTime,
                        EndTime,
                        AdjustmentAmount,
                        TotalAmmountToPay,
                        TotalAmmountPaid,
                        IsPaid,
                        CreatedOn,
                        UpdatedOn,
                        PaidOn
                    FROM Usage WITH (NOLOCK)
                    WHERE RoomId = @RoomId
                    ORDER BY CreatedOn DESC";
        var result = QueryText<Usage>(sql, new
        {
            RoomId = roomId,
            LastN = lastN
        });
        return result.ToList();
    }
}
