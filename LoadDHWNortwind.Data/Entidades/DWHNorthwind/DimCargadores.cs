﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LoadDHWNortwind.Data.Entidades.DWHNorthwind
{
    
    public class DimCargadores
    {
       
        public int DimCargadoresKey { get; set; }
        public int CargadorID { get; set; }
        public string Cargador { get; set; }
    }

}