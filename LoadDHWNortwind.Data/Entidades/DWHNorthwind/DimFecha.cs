using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace LoadDHWNortwind.Data.Entidades.DWHNorthwind
{
    public class DimFecha
    {
        public int DimFechaKey { get; set; }
        public int OrdenFecha { get; set; }
        public DateTime Fecha { get; set; }
        public string? NombreFecha { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
        public int Dia { get; set; }
        public int Trimestre { get; set; }
        public string? NombreMes { get; set; }
        public string? NombreDia { get; set; }
        public string? NombreTrimestre { get; set; }
        public string? FormatoCompleto { get; set; }
    }

}
