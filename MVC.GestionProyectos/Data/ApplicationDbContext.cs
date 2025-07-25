using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModelosOrganizacion;

namespace MVC.GestionProyectos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ModelosOrganizacion.Proyecto> Proyecto { get; set; } = default!;
        public DbSet<ModelosOrganizacion.Tarea> Tarea { get; set; } = default!;
    }
}
