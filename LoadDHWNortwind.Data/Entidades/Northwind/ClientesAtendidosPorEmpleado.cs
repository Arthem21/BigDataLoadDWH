﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadDHWNorthwind.Data.Entidades.Northwind
{
    public class ClientesAtendidosPorEmpleado
    {
        public int EmployeeID { get; set; }

        public string? Empleado { get; set; }

        public string? CantidadClientesUnicos { get; set; }
    }
}