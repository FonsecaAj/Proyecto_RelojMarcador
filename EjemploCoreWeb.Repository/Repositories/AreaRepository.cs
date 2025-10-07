using System.Data;
using Dapper;
using EjemploCoreWeb.Entities;
using EjemploCoreWeb.Repository.Interfaces;

namespace EjemploCoreWeb.Repository.Repositories;

public class AreaRepository : IAreaRepository
{
    private readonly IDbConnectionFactory _factory;
    public AreaRepository(IDbConnectionFactory factory) => _factory = factory;

    public async Task<IEnumerable<Area>> ListarAsync(string? filtro)
    {
        using var con = _factory.CreateConnection();
        var sql = @"
SELECT a.ID_Area, a.Nombre_Area, a.Jefe_Area, a.Codigo_Area,
       CONCAT(u.Nombre,' ',u.Apellido_1,' ',u.Apellido_2) AS Jefe_Nombre
FROM Areas a
LEFT JOIN Usuario u ON u.ID_Usuario = a.Jefe_Area
WHERE (@f IS NULL OR a.Nombre_Area LIKE CONCAT('%',@f,'%') OR a.Codigo_Area LIKE CONCAT('%',@f,'%'))
ORDER BY a.Nombre_Area;";
        return await con.QueryAsync<Area>(sql, new { f = string.IsNullOrWhiteSpace(filtro) ? null : filtro });
    }

    public async Task<Area?> ObtenerAsync(int id)
    {
        using var con = _factory.CreateConnection();
        var sql = @"SELECT ID_Area, Nombre_Area, Jefe_Area, Codigo_Area FROM Areas WHERE ID_Area=@id;";
        return await con.QueryFirstOrDefaultAsync<Area>(sql, new { id });
    }

    public async Task<int> CrearAsync(Area a)
    {
        using var con = _factory.CreateConnection();
        var sql = @"
INSERT INTO Areas (Nombre_Area, Jefe_Area, Codigo_Area)
VALUES (@Nombre_Area, @Jefe_Area, @Codigo_Area);
SELECT LAST_INSERT_ID();";
        return await con.ExecuteScalarAsync<int>(sql, a);
    }

    public async Task<bool> ActualizarAsync(Area a)
    {
        using var con = _factory.CreateConnection();
        var rows = await con.ExecuteAsync(@"
UPDATE Areas SET
 Nombre_Area=@Nombre_Area,
 Jefe_Area=@Jefe_Area,
 Codigo_Area=@Codigo_Area
WHERE ID_Area=@ID_Area;", a);
        return rows == 1;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        using var con = _factory.CreateConnection();
        try
        {
            var rows = await con.ExecuteAsync("DELETE FROM Areas WHERE ID_Area=@id;", new { id });
            return rows == 1;
        }
        catch (MySql.Data.MySqlClient.MySqlException ex) when (ex.Number == 1451)
        {
            return false;
        }
    }
}
