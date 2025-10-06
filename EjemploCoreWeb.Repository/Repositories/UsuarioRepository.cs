using System.Data;
using Dapper;
using EjemploCoreWeb.Entities;
using EjemploCoreWeb.Repository.Interfaces;
using MySqlConnector;

namespace EjemploCoreWeb.Repository.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IDbConnectionFactory _factory;
    public UsuarioRepository(IDbConnectionFactory factory) => _factory = factory;

    // ===== Helpers de WHERE =====
    private const string WhereFiltro = @"(@f IS NULL
        OR Identificacion LIKE CONCAT('%',@f,'%')
        OR Nombre         LIKE CONCAT('%',@f,'%')
        OR Apellido_1     LIKE CONCAT('%',@f,'%')
        OR Apellido_2     LIKE CONCAT('%',@f,'%')
        OR Correo         LIKE CONCAT('%',@f,'%')
        OR Nom_Usuario    LIKE CONCAT('%',@f,'%'))";

    private static object P(object? filtro, int? take = null, int? skip = null) =>
        new { f = string.IsNullOrWhiteSpace(filtro as string) ? null : filtro, take, skip };

    // ===== Listado simple =====
    public async Task<IEnumerable<Usuario>> ListarAsync(string? filtro)
    {
        using var con = _factory.CreateConnection();
        var sql = $@"
SELECT ID_Usuario, Identificacion, Nombre, Apellido_1, Apellido_2,
       Correo, Telefono, ID_Rol_Usuario, Nom_Usuario, Contrasena,
       Fecha_Creacion, Estado
FROM Usuario
WHERE {WhereFiltro}
ORDER BY Apellido_1, Apellido_2, Nombre;";
        return await con.QueryAsync<Usuario>(sql, P(filtro));
    }

    // ===== Paginado (page/size) =====
    public async Task<(IEnumerable<Usuario> Items, int Total)> ListarPaginadoAsync(string? filtro, int page, int size)
    {
        using var con = _factory.CreateConnection();

        var countSql = $"SELECT COUNT(*) FROM Usuario WHERE {WhereFiltro};";
        var total = await con.ExecuteScalarAsync<int>(countSql, P(filtro));

        var skip = (page - 1) * size;
        var dataSql = $@"
SELECT ID_Usuario, Identificacion, Nombre, Apellido_1, Apellido_2,
       Correo, Telefono, ID_Rol_Usuario, Nom_Usuario, Contrasena,
       Fecha_Creacion, Estado
FROM Usuario
WHERE {WhereFiltro}
ORDER BY Apellido_1, Apellido_2, Nombre
LIMIT @take OFFSET @skip;";
        var items = await con.QueryAsync<Usuario>(dataSql, P(filtro, size, skip));

        return (items, total);
    }

    // ===== Paginado (offset/size) + contador =====
    public async Task<IEnumerable<Usuario>> ListarPaginaAsync(string? filtro, int offset, int size)
    {
        using var con = _factory.CreateConnection();
        var sql = $@"
SELECT ID_Usuario, Identificacion, Nombre, Apellido_1, Apellido_2,
       Correo, Telefono, ID_Rol_Usuario, Nom_Usuario, Contrasena,
       Fecha_Creacion, Estado
FROM Usuario
WHERE {WhereFiltro}
ORDER BY Apellido_1, Apellido_2, Nombre
LIMIT @take OFFSET @skip;";
        return await con.QueryAsync<Usuario>(sql, P(filtro, size, offset));
    }

    public async Task<int> ContarAsync(string? filtro)
    {
        using var con = _factory.CreateConnection();
        var sql = $"SELECT COUNT(*) FROM Usuario WHERE {WhereFiltro};";
        return await con.ExecuteScalarAsync<int>(sql, P(filtro));
    }

    // ===== CRUD =====
    public async Task<Usuario?> ObtenerAsync(int id)
    {
        using var con = _factory.CreateConnection();
        var sql = @"
SELECT ID_Usuario, Identificacion, Nombre, Apellido_1, Apellido_2,
       Correo, Telefono, ID_Rol_Usuario, Nom_Usuario, Contrasena,
       Fecha_Creacion, Estado
FROM Usuario
WHERE ID_Usuario=@id
LIMIT 1;";
        return await con.QueryFirstOrDefaultAsync<Usuario>(sql, new { id });
    }

    public async Task<int> CrearAsync(Usuario u, string plainPassword)
    {
        using var con = _factory.CreateConnection();
        var hash = BCrypt.Net.BCrypt.HashPassword(plainPassword);
        var sql = @"
INSERT INTO Usuario
(Identificacion, Nombre, Apellido_1, Apellido_2, Correo, Telefono,
 ID_Rol_Usuario, Nom_Usuario, Contrasena, Estado)
VALUES
(@Identificacion, @Nombre, @Apellido_1, @Apellido_2, @Correo, @Telefono,
 @ID_Rol_Usuario, @Nom_Usuario, @Contrasena, @Estado);
SELECT LAST_INSERT_ID();";
        return await con.ExecuteScalarAsync<int>(sql, new
        {
            u.Identificacion,
            u.Nombre,
            u.Apellido_1,
            u.Apellido_2,
            u.Correo,
            u.Telefono,
            u.ID_Rol_Usuario,
            u.Nom_Usuario,
            Contrasena = hash,
            u.Estado
        });
    }

    public async Task<bool> ActualizarAsync(Usuario u, string? plainPassword)
    {
        using var con = _factory.CreateConnection();

        var setPass = "";
        object p = new
        {
            u.Identificacion,
            u.Nombre,
            u.Apellido_1,
            u.Apellido_2,
            u.Correo,
            u.Telefono,
            u.ID_Rol_Usuario,
            u.Nom_Usuario,
            u.Estado,
            u.ID_Usuario
        };

        if (!string.IsNullOrWhiteSpace(plainPassword))
        {
            setPass = ", Contrasena=@Contrasena";
            p = new
            {
                u.Identificacion,
                u.Nombre,
                u.Apellido_1,
                u.Apellido_2,
                u.Correo,
                u.Telefono,
                u.ID_Rol_Usuario,
                u.Nom_Usuario,
                u.Estado,
                Contrasena = BCrypt.Net.BCrypt.HashPassword(plainPassword),
                u.ID_Usuario
            };
        }

        var sql = $@"
UPDATE Usuario SET
 Identificacion=@Identificacion,
 Nombre=@Nombre,
 Apellido_1=@Apellido_1,
 Apellido_2=@Apellido_2,
 Correo=@Correo,
 Telefono=@Telefono,
 ID_Rol_Usuario=@ID_Rol_Usuario,
 Nom_Usuario=@Nom_Usuario,
 Estado=@Estado
 {setPass}
WHERE ID_Usuario=@ID_Usuario;";
        var rows = await con.ExecuteAsync(sql, p);
        return rows == 1;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        using var con = _factory.CreateConnection();
        try
        {
            var rows = await con.ExecuteAsync("DELETE FROM Usuario WHERE ID_Usuario=@id;", new { id });
            return rows == 1;
        }
        catch (MySqlException ex) when (ex.Number == 1451) // FK constraint
        {
            return false;
        }
    }

    // ===== Combos =====
    public async Task<IEnumerable<Rol>> ListarRolesAsync()
    {
        using var con = _factory.CreateConnection();
        const string sql = "SELECT ID_Rol_Usuario, Nombre_Rol FROM Roles ORDER BY Nombre_Rol;";
        return await con.QueryAsync<Rol>(sql);
    }

    public async Task<IEnumerable<TipoIdentificacion>> ListarTiposAsync()
    {
        using var con = _factory.CreateConnection();
        const string sql = @"
SELECT ID_Tipo_Identificacion, Tipo_Identificacion
FROM Tipos_Identificacion
ORDER BY Tipo_Identificacion;";
        return await con.QueryAsync<TipoIdentificacion>(sql);
    }
}
