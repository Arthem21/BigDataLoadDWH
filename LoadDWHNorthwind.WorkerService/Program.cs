using LoadDWHNorthwind.WorkerService;
using LoadDHWNortwind.Data.Interfaces;
using LoadDHWNortwind.Data.Contexto;
using Microsoft.EntityFrameworkCore;
using LoadDHWNortwind.Data.Servicios;

internal class Program
    {
        public static void Main(string[] args){

            CreateHostBuilder(args).Build().Run();
        }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
        services.AddDbContextPool<NorthwindContexto>(options =>
        options.UseSqlServer(hostContext.Configuration.GetConnectionString("Northwind")));

        services.AddDbContextPool<DWHNorthwindContexto>(options =>
        options.UseSqlServer(hostContext.Configuration.GetConnectionString("DWHNorthwind")));

        services.AddScoped<IServicioDatosDWHNorthwind, ServicioDatosDWHNorthwind>();

        services.AddHostedService<Worker>();
            
        });
    }
