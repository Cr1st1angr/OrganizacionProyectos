using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelosOrganizacion
{
    public class TareaProyecto
    {
        [Key]public int Id { get; set; }
        [ForeignKey(nameof(Tarea))] public int TareaId { get; set; }
        [ForeignKey(nameof(Proyecto))]public int ProyectoId { get; set; }
        public required string Estado { get; set; }
        public Tarea? Tarea { get; set; }
        public Proyecto? Proyecto { get; set; }
    }
}
