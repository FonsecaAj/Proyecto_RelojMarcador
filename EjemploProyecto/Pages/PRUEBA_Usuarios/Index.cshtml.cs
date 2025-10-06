using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using EjemploCoreWeb.Entities;
using EjemploCoreWeb.Services.Interfaces;

namespace EjemploProyecto.Pages.PRUEBA_Usuarios
{
    public class IndexModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;

        public IList<Usuario> Usuarios { get; private set; } = new List<Usuario>();

        [BindProperty(SupportsGet = true)]
        public string? Filtro { get; set; }

        public IndexModel(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public async Task OnGetAsync()
        {
            var data = await _usuarioService.ListarAsync(Filtro);
            Usuarios = data?.ToList() ?? new List<Usuario>();
        }
    }
}
