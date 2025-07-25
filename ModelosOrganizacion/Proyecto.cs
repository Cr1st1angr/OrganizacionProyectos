using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelosOrganizacion
{
    public class Proyecto
    {
        [Key]public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }

        public List<Tarea>? Tareas { get; set; }

    }
}
