using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ModelosOrganizacion;

namespace API.OrganizacionProyectos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly IConfiguration _config;

        public TareasController(IConfiguration context)
        {
            _config = context;
        }

        // GET: api/Tareas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarea>>> GetTarea()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"SELECT * FROM ""Tareas""";
            var tareas = connection.Query<Tarea>(sql).ToList();

            return tareas;
        }

        // GET: api/Tareas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tarea>> GetTareaById(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"SELECT * FROM ""Tareas"" WHERE ""Id"" = @Id";

            var tarea = connection.QueryFirstOrDefault<Tarea>(sql, new { Id = id });

            if (tarea == null)
            {
                return NotFound();
            }

            return tarea;
        }

        // PUT: api/Tareas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarea(int id, Tarea tarea)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"UPDATE ""Tareas"" SET ""Nombre"" = @Nombre, ""Funcionalidad"" = @Funcionalidad WHERE ""Id"" = @Id";


            if (id != tarea.Id)
            {
                return BadRequest();
            }


            try
            {
                connection.Execute(sql, new
                {
                    Id = tarea.Id,
                    Nombre = tarea.Nombre,
                    Funcionalidad = tarea.Funcionalidad
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Tareas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tarea>> PostTarea(Tarea tarea)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"INSERT INTO ""Tareas"" (""Nombre"", ""Funcionalidad"") VALUES (@Nombre, @Funcionalidad); SELECT CAST(SCOPE_IDENTITY() as int)";

            var idDevuelto = connection.ExecuteScalar<int>(sql, new
            {
                Nombre = tarea.Nombre,
                Funcionalidad = tarea.Funcionalidad
            });
            
            tarea.Id = idDevuelto;

            return CreatedAtAction(nameof(GetTareaById), new { id = idDevuelto }, tarea);
        }

        // DELETE: api/Tareas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarea(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"DELETE FROM ""Tareas"" WHERE ""Id"" = @Id";
            
            connection.Execute(sql, new { Id = id });

            return NoContent();
        }

    }
}
