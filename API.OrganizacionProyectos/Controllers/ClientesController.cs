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
    public class ClientesController : ControllerBase
    {
        private readonly IConfiguration _config;
        
        public ClientesController(IConfiguration config)
        {
            _config = config;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetCliente()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();
            var sql = @"SELECT * FROM ""Clientes""";

            var clientes = connection.Query<Cliente>(sql).ToList();

            return clientes;
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetClienteById(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();
            var sql = @"SELECT * FROM ""Clientes"" WHERE ""Id"" = @Id";
            var cliente = connection.QueryFirstOrDefault<Cliente>(sql, new { Id = id });

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        // PUT: api/Clientes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();
            var sql = @"UPDATE ""Clientes"" SET ""Nombre"" = @Nombre, ""Cedula"" = @Cedula, ""Email"" = @Email, ""Password""=@Password, ""UsuarioId""=@UsuarioId WHERE ""Id"" = @Id";
            if (id != cliente.Id)
            {
                return BadRequest();
            }

            try
            {
                connection.Execute(sql, new
                {
                    Nombre = cliente.Nombre,
                    Cedula = cliente.Cedula,
                    Email = cliente.Email,
                    Password = cliente.Password,
                    UsuarioId = cliente.UsuarioId,
                    Id = id
                });
            }
            catch (DbUpdateConcurrencyException)
            {

                    return NotFound();

            }

            return NoContent();
        }

        // POST: api/Clientes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();
            var sql = @"INSERT INTO ""Clientes"" (""Nombre"", ""Cedula"", ""Email"", ""Password"", ""UsuarioId"") 
                            VALUES (@Nombre, @Cedula, @Email, @Password, @UsuarioId);
                          SELECT CAST(SCOPE_IDENTITY() as int)";
            
            var idDevuelto = connection.ExecuteScalar<int>(sql, new
            {
                Nombre = cliente.Nombre,
                Cedula = cliente.Cedula,
                Email = cliente.Email,
                Password = cliente.Password,
                UsuarioId = cliente.UsuarioId
            });

            cliente.Id = idDevuelto;

            return CreatedAtAction(nameof(GetClienteById), new { id = idDevuelto }, cliente);
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OrganizacionProyectosContext"));
            connection.Open();

            var sql = @"DELETE FROM ""Clientes"" WHERE ""Id"" = @Id";

            connection.Execute(sql, new { Id = id });

            return NoContent();
        }

    }
}
