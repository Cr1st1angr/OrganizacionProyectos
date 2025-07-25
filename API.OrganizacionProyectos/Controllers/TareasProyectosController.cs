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
    public class TareasProyectosController : ControllerBase
    {
        private readonly IConfiguration _config;

        public TareasProyectosController(IConfiguration context)
        {
            _config = context;
        }

        // GET: api/TareasProyectos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TareaProyecto>>> GetTareaProyecto()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"SELECT * FROM ""TareasProyectos""";
            var tareasProyectos = connection.Query<TareaProyecto>(sql).ToList();
            return tareasProyectos;
        }

        // GET: api/TareasProyectos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TareaProyecto>> GetTareaProyectoById(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"SELECT * FROM ""TareasProyectos"" WHERE ""Id"" = @Id";

            var tareaProyecto = connection.QueryFirstOrDefault<TareaProyecto>(sql, new { Id = id });

            if (tareaProyecto == null)
            {
                return NotFound();
            }

            return tareaProyecto;
        }

        [HttpGet("TareasPorProyecto/{id}")]
        public async Task<ActionResult<IEnumerable<TareaProyecto>>> GetTareasPorProyecto(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"SELECT * FROM ""TareasProyectos"" WHERE ""ProyectoId"" = @Id";

            var tareaProyecto = connection.Query<TareaProyecto>(sql, new { Id = id }).ToList();

            if (tareaProyecto == null)
            {
                return NotFound();
            }

            return tareaProyecto;
        }

        // PUT: api/TareasProyectos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTareaProyecto(int id, TareaProyecto tareaProyecto)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"UPDATE ""TareasProyectos"" SET ""ProyectoId""=@ProyectoId, ""TareaId""=@TareaId WHERE ""Id""=@Id";


            if (id != tareaProyecto.Id)
            {
                return BadRequest();
            }

            try
            {
                connection.Execute(sql, new
                {
                    Id = tareaProyecto.Id,
                    ProyectoId = tareaProyecto.ProyectoId,
                    TareaId = tareaProyecto.TareaId
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/TareasProyectos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TareaProyecto>> PostTareaProyecto(TareaProyecto tareaProyecto)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"INSERT INTO ""TareasProyectos"" (""ProyectoId"", ""TareaId"", ""Estado"") VALUES (@ProyectoId, @TareaId, @Estado); SELECT CAST(SCOPE_IDENTITY() as int)";

            var idDevuelto = connection.ExecuteScalar<int>(sql, new
            {
                ProyectoId = tareaProyecto.ProyectoId,
                TareaId = tareaProyecto.TareaId,
                Estado = tareaProyecto.Estado
            });

            tareaProyecto.Id = idDevuelto;

            return CreatedAtAction(nameof(GetTareaProyectoById), new { id = idDevuelto }, tareaProyecto);
        }

        // DELETE: api/TareasProyectos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTareaProyecto(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"DELETE FROM ""TareasProyectos"" WHERE ""Id"" = @Id";

            connection.Execute(sql, new { Id = id });
            
            return NoContent();
        }

    }
}
