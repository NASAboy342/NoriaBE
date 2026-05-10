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
                     WaterPricePerUnit
                    from building ";
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
                     BuildingId
                    from room ";
        var result = QueryText<Room>(sql);
        return result.ToList();
    }

    public void AddBuilding(Building building)
    {
        var sql = @"INSERT INTO building (Name, Address, Img, Floors, ElectricityPricePerUnit, WaterPricePerUnit) 
                    VALUES (@Name, @Address, @Img, @Floors, @ElectricityPricePerUnit, @WaterPricePerUnit)";
        var result = QueryText<Building>(sql, new
        {
            Name = building.Name,
            Address = building.Address,
            Img = building.Img,
            Floors = building.Floors,
            ElectricityPricePerUnit = building.ElectricityPricePerUnit,
            WaterPricePerUnit = building.WaterPricePerUnit
        });
    }
    public void AddRoom(Room room)
    {
        var sql = @"INSERT INTO room (Name, BuildingId, IsOccupied, PhoneNumber, Floor, Price) 
                    VALUES (@Name, @BuildingId, @IsOccupied, @PhoneNumber, @Floor, @Price)";
        var result = QueryText<Room>(sql, new
        {
            Name = room.Name,
            BuildingId = room.BuildingId,
            IsOccupied = room.IsOccupied,
            PhoneNumber = room.PhoneNumber,
            Floor = room.Floor,
            Price = room.Price
        });
    }
    public void UpdateRoom(Room room)
    {
        var sql = @"UPDATE room 
                    SET Name = @Name, BuildingId = @BuildingId, IsOccupied = @IsOccupied, PhoneNumber = @PhoneNumber, Floor = @Floor, Price = @Price
                    WHERE Id = @Id";
        var result = QueryText<Room>(sql, new
        {
            Id = room.Id,
            Name = room.Name,
            BuildingId = room.BuildingId,
            IsOccupied = room.IsOccupied,
            PhoneNumber = room.PhoneNumber,
            Floor = room.Floor,
            Price = room.Price
        });
    }
    public void UpdateBuilding(Building building)
    {
        var sql = @"UPDATE building 
                    SET Name = @Name, Address = @Address, Img = @Img, Floors = @Floors, ElectricityPricePerUnit = @ElectricityPricePerUnit, WaterPricePerUnit = @WaterPricePerUnit
                    WHERE Id = @Id";
        var result = QueryText<Building>(sql, new
        {
            Id = building.Id,
            Name = building.Name,
            Address = building.Address,
            Img = building.Img,
            Floors = building.Floors,
            ElectricityPricePerUnit = building.ElectricityPricePerUnit,
            WaterPricePerUnit = building.WaterPricePerUnit
        });
    }
}
