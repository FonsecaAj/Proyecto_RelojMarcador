namespace EjemploCoreWeb.Entities;

public record class Area
{
    public int ID_Area { get; set; }
    public string Nombre_Area { get; set; } = "";
    public int Jefe_Area { get; set; }
    public string? Jefe_Nombre { get; set; } 
}

