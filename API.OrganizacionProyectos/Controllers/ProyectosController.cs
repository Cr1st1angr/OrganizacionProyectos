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
    public class ProyectosController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProyectosController(IConfiguration context)
        {
            _config = context;
        }

        // GET: api/Proyectos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proyecto>>> GetProyecto()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();
            var sql = @"SELECT * FROM ""Proyectos""";
            var proyectos = connection.Query<Proyecto>(sql).ToList();

            return proyectos;
        }

        // GET: api/Proyectos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Proyecto>> GetProyectoById(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();
            var sql = @"SELECT * FROM ""Proyectos"" WHERE ""Id"" = @Id";

            var proyecto = connection.QueryFirstOrDefault<Proyecto>(sql, new { Id = id });

            if (proyecto == null)
            {
                return NotFound();
            }

            return proyecto;
        }

        // PUT: api/Proyectos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProyecto(int id, Proyecto proyecto)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"UPDATE ""Proyectos"" SET ""Nombre"" = @Nombre, ""Descripcion"" = @Descripcion WHERE ""Id"" = @Id";
            
            if (id != proyecto.Id)
            {
                return BadRequest();
            }

            try
            {
                connection.Execute(sql, new
                {
                    Id = id,
                    Nombre = proyecto.Nombre,
                    Descripcion = proyecto.Descripcion
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Proyectos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Proyecto>> PostProyecto(Proyecto proyecto)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"INSERT INTO ""Proyectos"" (""Nombre"", ""Descripcion"") VALUES (@Nombre, @Descripcion); SELECT CAST(SCOPE_IDENTITY() as int)";

            var idDevuelto = connection.ExecuteScalar<int>(sql, new
            {
                Nombre = proyecto.Nombre,
                Descripcion = proyecto.Descripcion
            }); 

            proyecto.Id = idDevuelto;

            return CreatedAtAction(nameof(GetProyectoById), new { id = idDevuelto }, proyecto);
        }

        // DELETE: api/Proyectos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProyecto(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"DELETE FROM ""Proyectos"" WHERE ""Id"" = @Id";
            connection.Execute(sql, new { Id = id });
            
            return NoContent();
        }

    }
}
