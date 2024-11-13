using LoadDHWNortwind.Data.Entidades.DWHNorthwind;
using LoadDHWNortwind.Data.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadDHWNortwind.Data.Interfaces
{
    public interface IServicioDatosDWHNorthwind
    {
        Task<ResultadosOperacionales> CargarDimEmpleados();
        Task<ResultadosOperacionales> CargarDimClientes();
        Task<ResultadosOperacionales> CargarDimFecha();
        Task<ResultadosOperacionales> CargarDimProductos();
        Task<ResultadosOperacionales> CargarDimCargadores();

    }
}
