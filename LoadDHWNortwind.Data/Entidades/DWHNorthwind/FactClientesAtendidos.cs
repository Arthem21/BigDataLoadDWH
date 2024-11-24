using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LoadDHWNorthwind.Data.Entidades.DWHNorthwind
{
    public class FactClientesAtendidos
    {
        
        public int FactClientesAtendidosKey { get; set; }

        public int? EmpleadoID { get; set; }

        public string? Nombre { get; set; }

        public short? ClientesTotales { get; set; }
    }
}
