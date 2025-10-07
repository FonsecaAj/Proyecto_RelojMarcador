using System.Data;
using Dapper;
using BCrypt.Net;
using EjemploCoreWeb.Entities;
using EjemploCoreWeb.Repository.Interfaces;

namespace EjemploCoreWeb.Repository.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IDbConnectionFactory _factory;
    public UsuarioRepository(IDbConnectionFactory factory) => _factory = factory;

    public async Task<IEnumerable<Usuario>> ListarAsync(string? filtro)
    {
        using var con = _factory.CreateConnection();
        var sql = @"
SELECT ID_Usuario, Identificacion, Nombre, Apellido_1, Apellido_2,
       Correo, Telefono, ID_Rol_Usuario, Nom_Usuario, Contrasena,
       Fecha_Creacion, Estado
FROM Usuario
WHERE (@f IS NULL
       OR Identificacion LIKE CONCAT('%',@f,'%')
       OR Nombre LIKE CONCAT('%',@f,'%')
       OR Apellido_1 LIKE CONCAT('%',@f,'%')
       OR Apellido_2 LIKE CONCAT('%',@f,'%')
       OR Correo LIKE CONCAT('%',@f,'%')
       OR Nom_Usuario LIKE CONCAT('%',@f,'%'))
ORDER BY Apellido_1, Apellido_2, Nombre;";
        return await con.QueryAsync<Usuario>(sql, new { f = string.IsNullOrWhiteSpace(filtro) ? null : filtro });
    }

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
        catch (MySql.Data.MySqlClient.MySqlException ex) when (ex.Number == 1451) // FK constraint
        {
            return false;
        }
    }
}
