using EjemploCoreWeb.Entities;
// Usa este alias si tus interfaces están en "Interfaces".
// Si en tu proyecto están en "Abstract", cambia la línea de abajo por:
//   using Svc = EjemploCoreWeb.Services.Abstract;
using Svc = EjemploCoreWeb.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EjemploProyecto.Pages.Inconsistencias
{
    public class IndexModel : PageModel
    {
        private readonly Svc.IInconsistenciaService _service;
        private readonly Svc.IBitacoraService _bitacora;

        public IEnumerable<Inconsistencia> Inconsistencias { get; private set; }
            = System.Array.Empty<Inconsistencia>();

        public int TotalRegistros { get; private set; }

        // llega como ?pagina=#
        [BindProperty(SupportsGet = true)]
        public int PaginaActual { get; set; } = 1;

        public int TamanoPagina { get; } = 10;

        public IndexModel(Svc.IInconsistenciaService service, Svc.IBitacoraService bitacora)
        {
            _service = service;
            _bitacora = bitacora;
        }

        public async Task OnGetAsync()
        {
            Inconsistencias = await _service.Listar(PaginaActual, TamanoPagina)
                              ?? System.Array.Empty<Inconsistencia>();
            TotalRegistros = await _service.Contar();

            // bitacora
            await _bitacora.Registrar(1, 4, "Usuario consulto Inconsistencias", "CONSULTA");

            // mensajes para modal (exito / error)
            if (TempData.TryGetValue("SuccessMessage", out var ok))
            {
                TempData["ModalType"] = "success";
                TempData["ModalTitle"] = "Exito";
                TempData["ModalMessage"] = ok;
            }
            else if (TempData.TryGetValue("ErrorMessage", out var err))
            {
                TempData["ModalType"] = "error";
                TempData["ModalTitle"] = "Error";
                TempData["ModalMessage"] = err;
            }
        }
    }
}
