using ApiConsumer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelosOrganizacion;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC.GestionProyectos.Controllers
{
    
    public class ProyectosController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public ProyectosController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        // GET: Proyectos
        [Authorize(Roles ="desarrollador")]
        public async Task<ActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cliente = await Crud<Cliente>.GetClienteByUsuario(userId);

            var proyectos = await Crud<LiderProyecto>.GetProyectosPorLideres(cliente.Id);
            if (proyectos == null || proyectos.Count == 0)
            {
                var proyectosNoEncontrados = new List<LiderProyecto>();
                return View(proyectosNoEncontrados);
            }

            foreach(var proyecto in proyectos)
            {
                proyecto.Proyecto = await Crud<Proyecto>.GetByIdAsync(proyecto.ProyectoId);
            }

            ViewBag.Clientes = new List<Cliente>();

            return View(proyectos);
        }

        public async Task<ActionResult> IndexProyectosColaborativos()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cliente = await Crud<Cliente>.GetClienteByUsuario(userId);

            var proyectos = await Crud<LiderProyecto>.GetProyectosPorLideres(cliente.Id);
            if (proyectos == null || proyectos.Count == 0)
            {
                var proyectosNoEncontrados = new List<LiderProyecto>();
                return View(proyectosNoEncontrados);
            }

            foreach (var proyecto in proyectos)
            {
                proyecto.Proyecto = await Crud<Proyecto>.GetByIdAsync(proyecto.ProyectoId);
            }

            return View(proyectos);
        }

        // GET: Proyectos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [Authorize(Roles ="desarrollador")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proyectos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Proyecto proyecto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cliente = await Crud<Cliente>.GetClienteByUsuario(userId);
            try
            {
                var nuevoProyecto = new Proyecto
                {
                    Id = 0,
                    Nombre = proyecto.Nombre,
                    Descripcion = proyecto.Descripcion

                };

                var proyectoCreado = await Crud<Proyecto>.CreateAsync(nuevoProyecto);

                var liderProyecto = new LiderProyecto
                {
                    Id = 0,
                    ProyectoId = proyectoCreado.Id,
                    LiderId = cliente.Id // Asumiendo que el ID del líder es el ID del usuario
                };

                var user = await userManager.GetUserAsync(User);
                await userManager.AddToRoleAsync(user, "lider");

                var proyectoliderCreado = await Crud<LiderProyecto>.CreateAsync(liderProyecto);

                //await userManager.AddToRoleAsync(, "lider");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyectos/Edit/5
        public async Task<IActionResult> Invitar(int idProyecto)
        {
            Console.WriteLine($"Id del proyecto que llegó en invitar: {idProyecto}");
            ViewBag.IdProyecto = idProyecto;
            return View(new List<Cliente>());
        }

        [HttpGet]
        public async Task<ActionResult> CargarClientes(string correo)
        {

            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cliente = await Crud<Cliente>.GetClienteByUsuario(UserId);
            var clientes = await Crud<Cliente>.GetClientesPorCorreo(correo,cliente.Id);
            return View("Invitar", clientes);
        }

        public ActionResult InvitarMiembro(int id, int idProyecto)
        {
            var miembroProyecto = new ColaboradorProyecto
            {
                Id = 0,
                ProyectoId = idProyecto,
                ColaboradorId = id
            };

            var nuevo = Crud<ColaboradorProyecto>.CreateAsync(miembroProyecto);
            return RedirectToAction(nameof(Invitar), new { id = idProyecto});
        }

        // POST: Proyectos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyectos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proyectos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
