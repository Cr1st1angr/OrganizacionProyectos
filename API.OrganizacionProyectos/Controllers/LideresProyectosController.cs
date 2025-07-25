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
    public class LideresProyectosController : ControllerBase
    {
        private readonly IConfiguration _config;

        public LideresProyectosController(IConfiguration context)
        {
            _config = context;
        }

        // GET: api/LideresProyectos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LiderProyecto>>> GetLiderProyecto()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"SELECT * FROM ""LideresProyectos""";
            var lideresProyectos = connection.Query<LiderProyecto>(sql).ToList();

            return lideresProyectos;
        }

        // GET: api/LideresProyectos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LiderProyecto>> GetLiderProyectoById(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();
            var sql = @"SELECT * FROM ""LideresProyectos"" WHERE ""Id"" = @Id";
            var liderProyecto = connection.QueryFirstOrDefault<LiderProyecto>(sql, new { Id = id });

            if (liderProyecto == null)
            {
                return NotFound();
            }

            return liderProyecto;
        }

        [HttpGet("ProyectosLider/{idCliente}")]
        public async Task<ActionResult<IEnumerable<LiderProyecto>>> GetProyectosPorCliente(int idCliente)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();
            var sql = @"
                SELECT *
                FROM ""LideresProyectos"" WHERE ""LiderId"" = @LiderId";

            var lideresProyectos = connection.Query<LiderProyecto>(sql, new { LiderId = idCliente }).ToList();

            return lideresProyectos;

        }

        // PUT: api/LideresProyectos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLiderProyecto(int id, LiderProyecto liderProyecto)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"UPDATE ""LideresProyectos"" SET ""ProyectoId"" = @ProyectoId, ""LiderId"" = @LiderId WHERE ""Id"" = @Id";
            connection.Execute(sql, new
            {
                Id = id,
                ProyectoId = liderProyecto.ProyectoId,
                LiderId = liderProyecto.LiderId
            });
            if (id != liderProyecto.Id)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/LideresProyectos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LiderProyecto>> PostLiderProyecto(LiderProyecto liderProyecto)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();
            
            var ssql = @"INSERT INTO ""LideresProyectos"" (""ProyectoId"", ""LiderId"") VALUES (@ProyectoId, @LiderId); SELECT CAST(SCOPE_IDENTITY() as int)";
            var idDevuelto = connection.ExecuteScalar<int>(ssql, new
            {
                ProyectoId = liderProyecto.ProyectoId,
                LiderId = liderProyecto.LiderId
            });

            liderProyecto.Id = idDevuelto;
            return CreatedAtAction(nameof(GetLiderProyectoById), new { id = idDevuelto}, liderProyecto);
        }

        // DELETE: api/LideresProyectos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLiderProyecto(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"DELETE FROM ""LideresProyectos"" WHERE ""Id"" = @Id";

            connection.Execute(sql, new { Id = id });

            return NoContent();
        }

    }
}
