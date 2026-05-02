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
        _connectionString = configuration.GetSection("ConnnectionString").Value ?? string.Empty;
    }

    protected IEnumerable<T> Query<T>(string spName, object parameters = null, string database = "GameProviderDB", int timeoutTime = 180)
    {
        var cnnString = _connectionString;
        using var conn = new SqlConnection(cnnString);
        var sql = conn.Query<T>(spName, parameters, null, true, timeoutTime, CommandType.StoredProcedure);
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
        var sql = "";
        var result = Query<Building>(sql);
        return result.ToList();
    }
}
