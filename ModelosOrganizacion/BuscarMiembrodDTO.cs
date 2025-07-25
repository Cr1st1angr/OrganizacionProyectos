using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelosOrganizacion
{
    public class BuscarMiembrodDTO
    {
        public int idProyecto { get; set; }
        public List<Cliente> clientes { get; set; }
    }
}
