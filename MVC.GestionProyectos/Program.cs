using ApiConsumer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC.GestionProyectos.Data;
using ModelosOrganizacion;

namespace MVC.GestionProyectos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Crud<Cliente>.EndPoint = "https://localhost:7084/api/Clientes";
            Crud<Proyecto>.EndPoint = "https://localhost:7084/api/Proyectos";
            Crud<Tarea>.EndPoint = "https://localhost:7084/api/Tareas";
            Crud<TareaProyecto>.EndPoint = "https://localhost:7084/api/TareasProyectos";
            Crud<ColaboradorProyecto>.EndPoint = "https://localhost:7084/api/ColaboradoresProyectos";
            Crud<ColaboradorTarea>.EndPoint = "https://localhost:7084/api/ColaboradoresTareas";
            Crud<LiderProyecto>.EndPoint = "https://localhost:7084/api/LideresProyectos";

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>() // Permite el uso de roles de usuario
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();


            builder.Services.AddControllersWithViews();

            builder.Services.AddRazorPages();

            builder.Services.AddHttpClient();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            
            app.UseAuthentication();
            
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
