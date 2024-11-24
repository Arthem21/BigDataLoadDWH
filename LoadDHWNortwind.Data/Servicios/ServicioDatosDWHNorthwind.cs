using LoadDHWNortwind.Data.Entidades.DWHNorthwind;
using LoadDHWNortwind.Data.Interfaces;
using LoadDHWNortwind.Data.Resultados;
using LoadDHWNortwind.Data.Contexto;
using Microsoft.EntityFrameworkCore;
using LoadDHWNorthwind.Data.Entidades.DWHNorthwind;

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
            var resultado = new ResultadosOperacionales { Exito = true };

            try
            {
                var ordenes = await _northwindContexto.FactOrdenes.AsNoTracking().ToListAsync();

                int?[] ordersId = await _dwhNorthwindContexto.FactOrdenes.Select(cd => cd.OrdenID).ToArrayAsync();

                if (ordersId.Any())
                {
                    await _dwhNorthwindContexto.FactOrdenes
                        .Where(cd => ordersId.Contains(cd.OrdenID))
                        .ExecuteDeleteAsync();
                }

                List<FactOrdenes> factOrdenes = new List<FactOrdenes>();

                // Diccionarios para almacenar los resultados de las consultas
                var clientesCache = new Dictionary<string, DimClientes>();
                var empleadosCache = new Dictionary<int?, DimEmpleados>();
                var cargadoresCache = new Dictionary<int?, DimCargadores>();
                var productosCache = new Dictionary<int, DimProductos>();
                var fechasCache = new Dictionary<DateTime, DimFecha>();

                foreach (var orden in ordenes)
                {
                    DimClientes cliente = null;
                    if (!clientesCache.TryGetValue(orden.ClienteID, out cliente))
                    {
                        cliente = await _dwhNorthwindContexto.DimClientes
                            .AsNoTracking()
                            .FirstOrDefaultAsync(cli => cli.ClienteID == orden.ClienteID);
                        clientesCache[orden.ClienteID] = cliente;
                    }

                    DimEmpleados empleado = null;
                    if (orden.EmpleadoID.HasValue && !empleadosCache.TryGetValue(orden.EmpleadoID, out empleado))
                    {
                        empleado = await _dwhNorthwindContexto.DimEmpleados
                            .AsNoTracking()
                            .FirstOrDefaultAsync(emp => emp.EmpleadoID == orden.EmpleadoID.Value);
                        empleadosCache[orden.EmpleadoID] = empleado;
                    }

                    DimCargadores cargador = null;
                    if (orden.CargadorID.HasValue && !cargadoresCache.TryGetValue(orden.CargadorID, out cargador))
                    {
                        cargador = await _dwhNorthwindContexto.DimCargadores
                            .AsNoTracking()
                            .FirstOrDefaultAsync(car => car.CargadorID == orden.CargadorID.Value);
                        cargadoresCache[orden.CargadorID] = cargador;
                    }

                    DimProductos producto = null;
                    if (!productosCache.TryGetValue(orden.ProductoID, out producto))
                    {
                        producto = await _dwhNorthwindContexto.DimProductos
                            .AsNoTracking()
                            .FirstOrDefaultAsync(pro => pro.ProductoID == orden.ProductoID);
                        productosCache[orden.ProductoID] = producto;
                    }

                    DimFecha fecha = null;
                    if (orden.Fecha.HasValue && !fechasCache.TryGetValue(orden.Fecha.Value, out fecha))
                    {
                        fecha = await _dwhNorthwindContexto.DimFecha
                            .AsNoTracking()
                            .FirstOrDefaultAsync(fec => fec.Fecha == orden.Fecha.Value);
                        fechasCache[orden.Fecha.Value] = fecha;
                    }

                    if (producto != null && cliente != null && cargador != null && empleado != null && fecha != null)
                    {
                        FactOrdenes factorden = new FactOrdenes()
                        {
                            ClienteID = cliente.DimClienteKey,
                            EmpleadoID = empleado.DimEmpleadoKey,
                            Cargador = cargador.DimCargadoresKey,
                            ProductoID = producto.DimProductosKey,
                            FechaPedido = fecha?.DimFechaKey ?? 0, // Manejo de valores nulos
                            Precio = orden.TotalVentas,
                            Cantidad = Convert.ToInt16(orden.Cantidad),
                            Ciudad = orden.CiudadID,
                            Pais = orden.PaisID,
                            Region = orden.RegionID,
                            OrdenID = orden.OrdenID
                        };

                        factOrdenes.Add(factorden);
                    }
                }

                await _dwhNorthwindContexto.FactOrdenes.AddRangeAsync(factOrdenes);
                await _dwhNorthwindContexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                resultado.Exito = false;
                resultado.Mensaje = $"Error cargando el fact de Ordenes: {ex.Message}";
                if (ex.InnerException != null) 
                { 
                    resultado.Mensaje += $" Inner exception: {ex.InnerException.Message}"; 
                }

            }
              return resultado;
        }



        private async Task<ResultadosOperacionales> CargarClientesAtendidos()
        {
            var resultado = new ResultadosOperacionales { Exito = true };

            try
            {
                var CAE = await _northwindContexto.Customers_x_Employees.AsNoTracking().ToListAsync();

                int?[] CAEId = await _dwhNorthwindContexto.FactClientesAtendidos.AsNoTracking().Select(cae => cae.EmpleadoID).ToArrayAsync();

                if (CAEId.Any())
                {
                    await _dwhNorthwindContexto.FactClientesAtendidos.AsNoTracking()
                        .Where(cae => CAEId.Contains(cae.EmpleadoID))
                        .ExecuteDeleteAsync();
                }

                List<FactClientesAtendidos> factClientesAtendidos = new List<FactClientesAtendidos>();
                

                foreach (var cli in CAE)
                {
                    var empleado = await _dwhNorthwindContexto.DimEmpleados
                        .AsNoTracking()
                        .SingleOrDefaultAsync(emp => emp.EmpleadoID == cli.EmployeeID);

                    if (empleado != null)
                    {
                        FactClientesAtendidos CA = new FactClientesAtendidos()
                        {
                            
                            EmpleadoID = empleado.DimEmpleadoKey,
                            ClientesTotales = Convert.ToInt16(cli.CantidadClientesUnicos),
                            Nombre = empleado.Nombre
                        };

                        factClientesAtendidos.Add(CA);
                    }
                    
                }

                await _dwhNorthwindContexto.FactClientesAtendidos.AddRangeAsync(factClientesAtendidos);
                await _dwhNorthwindContexto.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                resultado.Exito = false;
                resultado.Mensaje = $"Error cargando el fact de ClientesAtendidos: {ex.Message}";
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
                //await CargarDimFecha();
                //await CargarDimProductos();
                await CargarClientesAtendidos();
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
