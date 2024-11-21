using LoadDHWNortwind.Data.Entidades.DWHNorthwind;
using LoadDHWNortwind.Data.Interfaces;
using LoadDHWNortwind.Data.Resultados;
using LoadDHWNortwind.Data.Contexto;
using LoadDHWNortwind.Data.Entidades.Northwind;
using Microsoft.EntityFrameworkCore;

namespace LoadDHWNortwind.Data.Servicios
{
    public class ServicioDatosDWHNorthwind : IServicioDatosDWHNorthwind
    {
        private readonly NorthwindContexto _northwindContexto;
        private readonly DWHNorthwindContexto _dwhNorthwindContexto;

        public ServicioDatosDWHNorthwind(NorthwindContexto northwindContexto, DWHNorthwindContexto dwhNorthwindContexto)
        {
            _northwindContexto = northwindContexto;
            _dwhNorthwindContexto = dwhNorthwindContexto;
        }

        private async Task<ResultadosOperacionales> CargarFactOrdenes()
        {
            var resultado = new ResultadosOperacionales { Exito = true }; try
            {
                var ordenes = await _northwindContexto.FactOrdenes.AsNoTracking().ToListAsync(); 
            } 
            catch (Exception ex) { 
                resultado.Exito = false; 
                resultado.Mensaje = $"Error cargando el fact de Ordenes: {ex.Message}"; 
            } return resultado; 
        }

        private async Task<ResultadosOperacionales> CargarClientesAtendidos()
        {
            var resultado = new ResultadosOperacionales { Exito = true }; try
            {
                var CAE = await _northwindContexto.ClientesAtendidosPorEmpleado.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                resultado.Exito = false;
                resultado.Mensaje = $"Error cargando el fact de Ordenes: {ex.Message}";
            }
            return resultado;
        }

        private async Task<ResultadosOperacionales> CargarDimCargadores()
        {
            var resultado = new ResultadosOperacionales { Exito = true };

            try
            {
                await _dwhNorthwindContexto.DimCargadores.ExecuteDeleteAsync();
                var cargadores = await _northwindContexto.Shippers.AsNoTracking()
                    .Select(ship => new DimCargadores
                    {
                        CargadorID = ship.ShipperID,
                        Cargador = ship.CompanyName
                    }).ToListAsync();

                await _dwhNorthwindContexto.DimCargadores.AddRangeAsync(cargadores);
                await _dwhNorthwindContexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                resultado.Exito = false;
                resultado.Mensaje = $"Error cargando la dimensión de cargadores: {ex.Message}";
            }

            return resultado;
        }

        private async Task<ResultadosOperacionales> CargarDimClientes()
        {
            var resultado = new ResultadosOperacionales { Exito = true };

            try
            {
                await _dwhNorthwindContexto.DimClientes.ExecuteDeleteAsync();
                var clientes = await _northwindContexto.Customers.AsNoTracking()
                    .Select(cliente => new DimClientes
                    {
                        ClienteID = cliente.CustomerID,
                        Compañia = cliente.CompanyName,
                        Contacto = cliente.ContactName
                    }).ToListAsync();

                await _dwhNorthwindContexto.DimClientes.AddRangeAsync(clientes);
                await _dwhNorthwindContexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                resultado.Exito = false;
                resultado.Mensaje = $"Error cargando la dimensión de clientes: {ex.Message}";
            }

            return resultado;
        }

        private async Task<ResultadosOperacionales> CargarDimEmpleados()
        {
            var resultado = new ResultadosOperacionales { Exito = true };

            try
            {
                await _dwhNorthwindContexto.DimEmpleados.ExecuteDeleteAsync();
                var empleados = await _northwindContexto.Employees.AsNoTracking()
                    .Select(emp => new DimEmpleados
                    {
                        EmpleadoID = emp.EmployeeID ?? 0,
                        Nombre = emp.FirstName,
                        Apellido = emp.LastName,
                        Titulo = emp.Title
                    }).ToListAsync();

                await _dwhNorthwindContexto.DimEmpleados.AddRangeAsync(empleados);
                await _dwhNorthwindContexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                resultado.Exito = false;
                resultado.Mensaje = $"Error cargando la dimensión de empleados: {ex.Message}";
            }

            return resultado;
        }

        private async Task<ResultadosOperacionales> CargarDimFecha()
        {
            var resultado = new ResultadosOperacionales { Exito = true };

            try
            {
                await _dwhNorthwindContexto.DimFecha.ExecuteDeleteAsync();
                var fechas = await _northwindContexto.Orders.AsNoTracking()
                    .Select(order => new DimFecha
                    {
                        OrdenFecha = order.OrderDate.HasValue ? Convert.ToInt32(order.OrderDate.Value.ToString("yyyyMMdd")) : 0,
                        Fecha = order.OrderDate ?? DateTime.MinValue,
                        NombreFecha = order.OrderDate.HasValue ? order.OrderDate.Value.ToString("dd/MM/yyyy") : "Fecha no disponible",
                        Año = order.OrderDate.HasValue ? order.OrderDate.Value.Year : 0,
                        Mes = order.OrderDate.HasValue ? order.OrderDate.Value.Month : 0,
                        Dia = order.OrderDate.HasValue ? order.OrderDate.Value.Day : 0,
                        Trimestre = order.OrderDate.HasValue ? (order.OrderDate.Value.Month - 1) / 3 + 1 : 0,
                        NombreMes = order.OrderDate.HasValue ? order.OrderDate.Value.ToString("MMMM") : "Mes no disponible",
                        NombreDia = order.OrderDate.HasValue ? order.OrderDate.Value.DayOfWeek.ToString() : "Día no disponible",
                        NombreTrimestre = order.OrderDate.HasValue ? "Q" + ((order.OrderDate.Value.Month - 1) / 3 + 1).ToString() : "Trimestre no disponible",
                        FormatoCompleto = order.OrderDate.HasValue ? order.OrderDate.Value.ToString("F") : "Fecha no disponible"
                    }).ToListAsync();

                await _dwhNorthwindContexto.DimFecha.AddRangeAsync(fechas);
                await _dwhNorthwindContexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                resultado.Exito = false;
                resultado.Mensaje = $"Error cargando la dimensión de fechas: {ex.Message}";
            }

            return resultado;
        }

        private async Task<ResultadosOperacionales> CargarDimProductos()
        {
            var resultado = new ResultadosOperacionales { Exito = true };

            try
            {
                await _dwhNorthwindContexto.DimProductos.ExecuteDeleteAsync();
                var productos = await (from product in _northwindContexto.Products
                                       join category in _northwindContexto.Categories on product.CategoryID equals category.CategoryID
                                       select new DimProductos
                                       {
                                           CategoriaID = category.CategoryID,
                                           ProductoID = product.ProductID,
                                           Categoria = category.CategoryName,
                                           Producto = product.ProductName,
                                           Precio = product.UnitPrice ?? 0,
                                           Cantidad = product.UnitsInStock ?? 0
                                       }).AsNoTracking().ToListAsync();

                await _dwhNorthwindContexto.DimProductos.AddRangeAsync(productos);
                await _dwhNorthwindContexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                resultado.Exito = false;
                resultado.Mensaje = $"Error cargando la dimensión de productos: {ex.Message}";
            }

            return resultado;
        }

        public async Task<ResultadosOperacionales> CargarDWH()
        {
            var resultado = new ResultadosOperacionales { Exito = true };

            try
            {
                //await CargarDimCargadores();
                //await CargarDimClientes();
                //await CargarDimEmpleados();
                await CargarClientesAtendidos();
                //await CargarDimFecha();
                //await CargarDimProductos();
                await CargarFactOrdenes();
            }
            catch (Exception ex)
            {
                resultado.Exito = false;
                resultado.Mensaje = $"Error cargando el DWH: {ex.Message}";
            }

            return resultado;
        }
    }
}
