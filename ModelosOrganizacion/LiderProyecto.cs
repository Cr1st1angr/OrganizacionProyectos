using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelosOrganizacion
{
    public class LiderProyecto
    {
        [Key]public int Id { get; set; }
        [ForeignKey(nameof(Proyecto))]public int ProyectoId { get; set; }
        [ForeignKey(nameof(Cliente))]public int LiderId { get; set; }
        public Proyecto? Proyecto { get; set; }
        public Cliente? Cliente { get; set; }

    }
}
