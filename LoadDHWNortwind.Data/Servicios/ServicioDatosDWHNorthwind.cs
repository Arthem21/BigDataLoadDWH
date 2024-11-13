using LoadDHWNortwind.Data.Entidades.DWHNorthwind;
using LoadDHWNortwind.Data.Interfaces;
using LoadDHWNortwind.Data.Resultados;
using LoadDHWNortwind.Data.Contexto;
using LoadDHWNortwind.Data.Entidades.Northwind;

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

        public async Task<ResultadosOperacionales> CargarDimCargadores()
        {
            ResultadosOperacionales resultado = new ResultadosOperacionales();
            resultado.Exito = true;

            try
            {
                var cargadores = _northwindContexto.Shippers.Select(ship => new DimCargador
                {
                    CargadorID = ship.ShipperID,
                    Cargador = ship.CompanyName
                }).ToList();

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

        public async Task<ResultadosOperacionales> CargarDimClientes()
        {
            ResultadosOperacionales resultado = new ResultadosOperacionales();

            try
            {
                var clientes = _northwindContexto.Customers.Select(cliente => new DimCliente
                {
                    ClienteID = cliente.CustomerID,
                    Compañia = cliente.CompanyName,
                    Contacto = cliente.ContactName
                }).ToList();

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

        public async Task<ResultadosOperacionales> CargarDimEmpleados()
        {
            ResultadosOperacionales resultado = new ResultadosOperacionales();

            try
            {
                var empleados = _northwindContexto.Employees.Select(emp => new DimEmpleado
                {
                    EmpleadoID = emp.EmployeeID,
                    Nombre = emp.FirstName,
                    Apellido = emp.LastName,
                    Titulo = emp.Title
                }).ToList();

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

        public async Task<ResultadosOperacionales> CargarDimFecha()
        {
            ResultadosOperacionales resultado = new ResultadosOperacionales();

            try
            {
                var fechas = _northwindContexto.Orders.Select(order => new DimFecha
                {
                    OrdenFecha = Convert.ToInt32(order.OrderDate),
                    Fecha = order.OrderDate,
                    NombreFecha = order.OrderDate.ToString("D"),
                    Año = order.OrderDate.Year,
                    Mes = order.OrderDate.Month,
                    Dia = order.OrderDate.Day,
                    Trimestre = (order.OrderDate.Month - 1) / 3 + 1,
                    NombreMes = order.OrderDate.ToString("MMMM"),
                    NombreDia = order.OrderDate.DayOfWeek.ToString(),
                    NombreTrimestre = "Q" + (order.OrderDate.Month - 1) / 3 + 1.ToString(),
                    FormatoCompleto = order.OrderDate.ToString("F")

                }).ToList();

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

        public async Task<ResultadosOperacionales> CargarDimProductos()
        {
            ResultadosOperacionales resultado = new ResultadosOperacionales();

            try
            {
                var query = (from Product in _northwindContexto.Products join
                            Category in _northwindContexto.Categories on
                            Product.CategoryID equals Category.CategoryID
                            select new DimProducto()
                            {
                                CategoriaID = Category.CategoryID,
                                ProductoID = Product.ProductID,
                                Categoria= Category.CategoryName,
                                Producto=Product.ProductName,
                                Precio=Product.UnitPrice,
                                Cantidad=Product.UnitsInStock
                            }).ToList();


                await _dwhNorthwindContexto.DimProductos.AddRangeAsync(query);
                await _dwhNorthwindContexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                resultado.Exito = false;
                resultado.Mensaje = $"Error cargando la dimensión de productos: {ex.Message}";
            }

            return resultado;
        }
    }
}
