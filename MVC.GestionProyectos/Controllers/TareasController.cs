using ApiConsumer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ModelosOrganizacion;
using System.Security.Claims;

namespace MVC.GestionProyectos.Controllers
{
    public class TareasController : Controller
    {
        // GET: Tareas
        public async Task<ActionResult> Index(int id)
        {
            Console.WriteLine($"Id del proyecto: {id}");
            var tareas = await Crud<TareaProyecto>.GetTareasPorProyecto(id);

            foreach(var tarea in tareas)
            {
                tarea.Tarea = await Crud<Tarea>.GetByIdAsync(tarea.TareaId);
            }
            ViewBag.IdProyecto = id;
            return View(tareas);
        }
        public async Task<ActionResult> TareasDesarrollador()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cliente = await Crud<Cliente>.GetClienteByUsuario(userId);
            var tareas = await Crud<ColaboradorTarea>.GetTareasColaboradores(cliente.Id);

            foreach (var tarea in tareas)
            {
                tarea.TareaProyecto = await Crud<TareaProyecto>.GetByIdAsync(tarea.TareaProyectoId);
            }

            foreach(var tarea in tareas)
            {
                tarea.TareaProyecto.Tarea = await Crud<Tarea>.GetByIdAsync(tarea.TareaProyecto.TareaId);
            }

            return View(tareas);

        }

        // GET: Tareas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tareas/Create
        public ActionResult Create(int idProyecto)
        {
            ViewBag.IdProyecto = idProyecto;
            return View();
        }

        // POST: Tareas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int idProyecto,Tarea tarea)
        {
            try
            {
                var tareanueva = new Tarea
                {
                    Id = tarea.Id,
                    Nombre = tarea.Nombre,
                    Funcionalidad = tarea.Funcionalidad
                };

                var tareaCreada = await Crud<Tarea>.CreateAsync(tareanueva);
                Console.WriteLine($"Id de proyecto: {idProyecto}");
                var tareaProyecto = new TareaProyecto
                {
                    Id = 0,
                    ProyectoId = idProyecto,
                    TareaId = tareaCreada.Id,
                    Estado = "Pendiente"
                };

                var nuevaTareaProyecto = await Crud<TareaProyecto>.CreateAsync(tareaProyecto);
                return RedirectToAction(nameof(Index), new {id = idProyecto});
            }
            catch
            {
                return View();
            }
        }

        // GET: Tareas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Tareas/Edit/5
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

        // GET: Tareas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Tareas/Delete/5
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
