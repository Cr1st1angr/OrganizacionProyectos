using ApiConsumer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelosOrganizacion;

namespace MVC.GestionProyectos.Controllers
{
    public class TareasController : Controller
    {
        // GET: Tareas
        public async Task<ActionResult> Index(int id)
        {
            var tareas = await Crud<TareaProyecto>.GetTareasPorProyecto(id);
            ViewBag.IdProyecto = id;
            if (tareas == null || tareas.Count == 0)
            {
                var tareasNoEncontradas = new List<TareaProyecto>();
                return View(tareasNoEncontradas);
            }

            foreach(var tarea in tareas)
            {
                tarea.Tarea = await Crud<Tarea>.GetByIdAsync(tarea.TareaId);
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
        public ActionResult Create(int idProyecto,Tarea tarea)
        {
            try
            {
                var tareanueva = new Tarea
                {
                    Id = tarea.Id,
                    Nombre = tarea.Nombre,
                    Funcionalidad = tarea.Funcionalidad
                };

                var tareaCreada = Crud<Tarea>.CreateAsync(tareanueva);
                Console.WriteLine($"Id de proyecto: {idProyecto}");
                var tareaProyecto = new TareaProyecto
                {
                    Id = 0,
                    ProyectoId = idProyecto,
                    TareaId = tareaCreada.Id,
                    Estado = "Pendiente"
                };

                var nuevaTareaProyecto = Crud<TareaProyecto>.CreateAsync(tareaProyecto);
                return RedirectToAction(nameof(Index));
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
