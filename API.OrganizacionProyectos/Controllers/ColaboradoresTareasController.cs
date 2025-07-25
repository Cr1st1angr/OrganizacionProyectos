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
    public class ColaboradoresTareasController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ColaboradoresTareasController(IConfiguration context)
        {
            _config = context;
        }

        // GET: api/ColaboradoresTareas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColaboradorTarea>>> GetColaboradorTarea()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"SELECT * FROM ""ColaboradoresTareas""";
            var colaboradoresTareas = connection.Query<ColaboradorTarea>(sql).ToList();

            return colaboradoresTareas;
        }

        // GET: api/ColaboradoresTareas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ColaboradorTarea>> GetColaboradorTareaById(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"SELECT * FROM ""ColaboradoresTareas"" WHERE ""Id"" = @Id";

            var colaboradorTarea = connection.QueryFirstOrDefault<ColaboradorTarea>(sql, new { Id = id });

            if (colaboradorTarea == null)
            {
                return NotFound();
            }

            return colaboradorTarea;
        }

        // PUT: api/ColaboradoresTareas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColaboradorTarea(int id, ColaboradorTarea colaboradorTarea)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"update ""ColaboradoresTareas"" set ""ColaboradorId"" = @ColaboradorId, ""TareaProyectoId"" = @TareaId where ""Id"" = @Id";
            connection.Execute(sql, new
            {
                ColaboradorId = colaboradorTarea.ColaboradorId,
                TareaId = colaboradorTarea.TareaProyectoId,
                Id = id
            });

            if (id != colaboradorTarea.Id)
            {
                return BadRequest();
            }
         

            return NoContent();
        }

        // POST: api/ColaboradoresTareas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ColaboradorTarea>> PostColaboradorTarea(ColaboradorTarea colaboradorTarea)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"INSERT INTO ""ColaboradoresTareas"" (""ColaboradorId"", ""TareaProyectoId"") 
                          VALUES (@ColaboradorId, @TareaProyectoId) ;
                          SELECT CAST(SCOPE_IDENTITY() as int";
            
            var idDevuelo = connection.ExecuteScalar<int>(sql, new
            {
                ColaboradorId = colaboradorTarea.ColaboradorId,
                TareaProyectoId = colaboradorTarea.TareaProyectoId
            });

            colaboradorTarea.Id = idDevuelo;

            return CreatedAtAction(nameof(GetColaboradorTareaById), new { id = idDevuelo }, colaboradorTarea);
        }

        // DELETE: api/ColaboradoresTareas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColaboradorTarea(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();
            var sql = @"DELETE FROM ""ColaboradoresTareas"" WHERE ""Id"" = @Id";

            connection.Execute(sql, new { Id = id });
           
            return NoContent();
        }
    }
}
