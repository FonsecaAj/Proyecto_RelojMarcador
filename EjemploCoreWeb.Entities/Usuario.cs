namespace EjemploCoreWeb.Entities;

public class Usuario
{
    public int ID_Usuario { get; set; }
    public string Identificacion { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Apellido_1 { get; set; } = "";
    public string Apellido_2 { get; set; } = "";
    public string Correo { get; set; } = "";
    public string? Telefono { get; set; }
    public int ID_Rol_Usuario { get; set; }
    public string Nom_Usuario { get; set; } = "";
    public string Contrasena { get; set; } = ""; // hash en BD
    public DateTime Fecha_Creacion { get; set; }
    public string Estado { get; set; } = "Activo"; // ENUM en MySQL

    public string NombreCompleto => $"{Nombre} {Apellido_1} {Apellido_2}".Trim();
}
