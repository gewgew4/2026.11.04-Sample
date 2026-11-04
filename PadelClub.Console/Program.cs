using System.Globalization;
using System.Text;
using Consola.Menus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PadelClub.Application.Canchas;
using PadelClub.Application.Reservas;
using PadelClub.Application.Socios;
using PadelClub.Infrastructure;
using Serilog;

namespace Consola;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        var cultura = new CultureInfo("es-AR");
        CultureInfo.CurrentCulture = cultura;
        CultureInfo.CurrentUICulture = cultura;

        var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings { Args = args, ContentRootPath = AppContext.BaseDirectory });
        var logsDirectory = Path.Combine(FindSolutionRoot(AppContext.BaseDirectory), "Logs");
        Directory.CreateDirectory(logsDirectory);
        builder.Configuration["Serilog:WriteTo:0:Args:path"] = Path.Combine(logsDirectory, "log-.txt");

        // Se sacan los providers de consola. Serilog escribe los eventos en Logs/*.txt.
        builder.Logging.ClearProviders();
        builder.Services.AddSerilog((_, loggerConfiguration) =>
            loggerConfiguration.ReadFrom.Configuration(builder.Configuration));

        // Sin cadena no se corta acá: la falla aparece al usar la base y la captura el try/catch del loop.
        var cadena = builder.Configuration.GetConnectionString("PadelClub") ?? "";
        builder.Services.AgregarInfraestructura(cadena);
        builder.Services.AddScoped<CanchaServicio>();
        builder.Services.AddScoped<SocioServicio>();
        builder.Services.AddScoped<ReservaServicio>();
        builder.Services.AddScoped<MenuCanchas>();
        builder.Services.AddScoped<MenuSocios>();
        builder.Services.AddScoped<MenuReservas>();

        using var host = builder.Build();
        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("PadelClub application started.");

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("===================================");
            Console.WriteLine("        PADELCLUB v1.0");
            Console.WriteLine("===================================");
            Console.WriteLine(" 1. Canchas");
            Console.WriteLine(" 2. Socios");
            Console.WriteLine(" 3. Reservas");
            Console.WriteLine(" 0. Salir");
            Console.WriteLine("-----------------------------------");
            var opcion = Lector.LeerTexto("Opción");
            if (opcion == "0")
                return;

            // Único try/catch. Una regla de negocio ya volvió como Result y no entra acá.
            // SQL caído, timeout u otra excepción se loguean y el menú sigue.
            // El scope es por opción: si SaveChanges falla, ese DbContext se descarta con el alcance.
            try
            {
                using var alcance = host.Services.CreateScope();
                var proveedor = alcance.ServiceProvider;
                switch (opcion)
                {
                    case "1":
                        await proveedor.GetRequiredService<MenuCanchas>().EjecutarAsync();
                        break;
                    case "2":
                        await proveedor.GetRequiredService<MenuSocios>().EjecutarAsync();
                        break;
                    case "3":
                        await proveedor.GetRequiredService<MenuReservas>().EjecutarAsync();
                        break;
                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falla técnica en la opción {Opción}.", opcion);
                Console.WriteLine("No se pudo completar la operación. El sistema sigue disponible.");
            }
        }

    static string FindSolutionRoot(string startPath)
    {
        for (var directory = new DirectoryInfo(startPath); directory is not null; directory = directory.Parent)
        {
            if (directory.EnumerateFiles("*.sln").Any())
                return directory.FullName;
        }

        return startPath;
    }
    }
}
