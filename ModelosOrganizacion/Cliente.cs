using System.ComponentModel.DataAnnotations;

namespace ModelosOrganizacion
{
    public class Cliente
    {
        [Key] public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Cedula { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string  UsuarioId { get; set; }

        public List<Proyecto>? Proyectos { get; set; } 
        public List<TareaProyecto>? Tareas { get; set; }
    }
}
