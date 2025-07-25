using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelosOrganizacion;

    public class OrganizacionProyectosContext : DbContext
    {
        public OrganizacionProyectosContext (DbContextOptions<OrganizacionProyectosContext> options)
            : base(options)
        {
        }

        public DbSet<ModelosOrganizacion.Cliente> Clientes { get; set; } = default!;

public DbSet<ModelosOrganizacion.Proyecto> Proyectos { get; set; } = default!;

public DbSet<ModelosOrganizacion.Tarea> Tareas { get; set; } = default!;

public DbSet<ModelosOrganizacion.LiderProyecto> LideresProyectos { get; set; } = default!;

public DbSet<ModelosOrganizacion.ColaboradorProyecto> ColaboradoresProyectos { get; set; } = default!;

public DbSet<ModelosOrganizacion.TareaProyecto> TareasProyectos { get; set; } = default!;

public DbSet<ModelosOrganizacion.ColaboradorTarea> ColaboradoresTareas { get; set; } = default!;
    }
