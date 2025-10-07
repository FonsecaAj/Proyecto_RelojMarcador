using EjemploCoreWeb.Entities;
using EjemploCoreWeb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EjemploProyecto.Pages.Admin.Usuarios;

public class EditModel : PageModel
{
    private readonly IUsuarioService _svc;
    public EditModel(IUsuarioService svc) => _svc = svc;

    public SelectList TiposSelectList { get; set; } = default!;
    public SelectList RolesSelectList { get; set; } = default!;

    public class VM
    {
        [Required] public int ID_Usuario { get; set; }
        [Required] public int ID_Tipo_Identificacion { get; set; } // UI-only
        [Required, StringLength(30)] public string Identificacion { get; set; } = "";
        [Required, StringLength(50), RegularExpression("^[A-Za-zÁÉÍÓÚáéíóúñÑ ]+$")] public string Nombre { get; set; } = "";
        [Required, StringLength(50), RegularExpression("^[A-Za-zÁÉÍÓÚáéíóúñÑ ]+$")] public string Apellido_1 { get; set; } = "";
        [Required, StringLength(50), RegularExpression("^[A-Za-zÁÉÍÓÚáéíóúñÑ ]+$")] public string Apellido_2 { get; set; } = "";
        [Required, EmailAddress] public string Correo { get; set; } = "";
        public string? Telefono { get; set; }
        [Required] public int ID_Rol_Usuario { get; set; }
        [Required] public string Nom_Usuario { get; set; } = "";
        public string? Password { get; set; } // opcional
        [Required] public string Estado { get; set; } = "Activo";
    }

    [BindProperty] public VM Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var u = await _svc.ObtenerAsync(id);
        if (u is null) return RedirectToPage("Index");

        RolesSelectList = new SelectList(await _svc.ListarRolesAsync(), "ID_Rol_Usuario", "Nombre_Rol", u.ID_Rol_Usuario);
        TiposSelectList = new SelectList(await _svc.ListarTiposAsync(), "ID_Tipo_Identificacion", "Tipo_Identificacion");

        Input = new VM
        {
            ID_Usuario = u.ID_Usuario,
            ID_Tipo_Identificacion = 1, // UI-only (no se persiste)
            Identificacion = u.Identificacion,
            Nombre = u.Nombre,
            Apellido_1 = u.Apellido_1,
            Apellido_2 = u.Apellido_2,
            Correo = u.Correo,
            Telefono = u.Telefono,
            ID_Rol_Usuario = u.ID_Rol_Usuario,
            Nom_Usuario = u.Nom_Usuario,
            Estado = u.Estado
        };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            RolesSelectList = new SelectList(await _svc.ListarRolesAsync(), "ID_Rol_Usuario", "Nombre_Rol", Input.ID_Rol_Usuario);
            TiposSelectList = new SelectList(await _svc.ListarTiposAsync(), "ID_Tipo_Identificacion", "Tipo_Identificacion");
            return Page();
        }

        var u = new Usuario
        {
            ID_Usuario = Input.ID_Usuario,
            Identificacion = Input.Identificacion,
            Nombre = Input.Nombre,
            Apellido_1 = Input.Apellido_1,
            Apellido_2 = Input.Apellido_2,
            Correo = Input.Correo,
            Telefono = Input.Telefono,
            ID_Rol_Usuario = Input.ID_Rol_Usuario,
            Nom_Usuario = Input.Nom_Usuario,
            Estado = Input.Estado
        };

        var ok = await _svc.ActualizarAsync(u, string.IsNullOrWhiteSpace(Input.Password) ? null : Input.Password);
        TempData[ok ? "Ok" : "Error"] = ok ? "Usuario actualizado." : "No se pudo actualizar.";
        return RedirectToPage("Index");
    }
}
