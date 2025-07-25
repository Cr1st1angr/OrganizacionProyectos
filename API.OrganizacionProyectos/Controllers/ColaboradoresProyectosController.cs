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
    public class ColaboradoresProyectosController : ControllerBase
    {
        private readonly IConfiguration _config;
        public ColaboradoresProyectosController(IConfiguration context)
        {
            _config = context;
        }

        // GET: api/ColaboradoresProyectos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColaboradorProyecto>>> GetColaboradorProyecto()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"SELECT * FROM ""ColaboradoresProyectos""";
            var colaboradoresproyectos = connection.Query<ColaboradorProyecto>(sql).ToList();
            return colaboradoresproyectos;
        }

        // GET: api/ColaboradoresProyectos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ColaboradorProyecto>> GetColaboradorProyectoById(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();
            
            var sql = @"SELECT * FROM ""ColaboradoresProyectos"" WHERE ""Id"" = @Id";

            var colaboradorproyecto = connection.QueryFirstOrDefault<ColaboradorProyecto>(sql, new { Id = id });

            if (colaboradorproyecto == null)
            {
                return NotFound();
            }

            return colaboradorproyecto;
        }

        // PUT: api/ColaboradoresProyectos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColaboradorProyecto(int id, ColaboradorProyecto colaboradorProyecto)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"UPDATE ""ColaboradoresProyectos"" SET ""ColaboradorId"" = @ColaboradorId, ""ProyectoId"" = @ProyectoId WHERE ""Id"" = @Id";

            
            if (id != colaboradorProyecto.Id)
            {
                return BadRequest();
            }

            try
            {
                connection.Execute(sql, new
                {
                    Id = id,
                    ColaboradorId = colaboradorProyecto.ColaboradorId,
                    ProyectoId = colaboradorProyecto.ProyectoId
                });
            }
            catch (DbUpdateConcurrencyException)
            {

                    return NotFound();
            }

            return NoContent();
        }

        // POST: api/ColaboradoresProyectos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ColaboradorProyecto>> PostColaboradorProyecto(ColaboradorProyecto colaboradorProyecto)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"INSERT INTO ""ColaboradoresProyectos"" (""ColaboradorId"", ""ProyectoId"") 
                          VALUES (@ColaboradorId, @ProyectoId);
                          SELECT CAST(SCOPE_IDENTITY() as int";

            var idDevuelto = connection.ExecuteScalar<int>(sql, new
            {
                ColaboradorId = colaboradorProyecto.ColaboradorId,
                ProyectoId = colaboradorProyecto.ProyectoId
            });

            colaboradorProyecto.Id = idDevuelto;

            return CreatedAtAction(nameof(GetColaboradorProyectoById), new { id = idDevuelto }, colaboradorProyecto);
        }

        // DELETE: api/ColaboradoresProyectos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColaboradorProyecto(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"DELETE FROM ""ColaboradoresProyectos"" WHERE ""Id"" = @Id";

            connection.Execute(sql, new { Id = id });

            return NoContent();
        }
    }
}
