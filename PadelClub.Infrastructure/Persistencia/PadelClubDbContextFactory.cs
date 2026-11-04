using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PadelClub.Infrastructure.Persistencia;

// Lo usa `dotnet ef`, no el menú. Sube directorios hasta encontrar PadelClub.Console/appsettings.json
// y crea el DbContext con esa cadena. El Factory Method del parcial es Reserva.Crear.
public sealed class PadelClubDbContextFactory : IDesignTimeDbContextFactory<PadelClubDbContext>
{
    public PadelClubDbContext CreateDbContext(string[] args)
    {
        using var documento = JsonDocument.Parse(File.ReadAllText(BuscarAppSettings()));
        var cadena = documento.RootElement.GetProperty("ConnectionStrings").GetProperty("PadelClub").GetString();

        var opciones = new DbContextOptionsBuilder<PadelClubDbContext>()
            .UseSqlServer(cadena)
            .Options;

        return new PadelClubDbContext(opciones);
    }

    static string BuscarAppSettings()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var candidato = Path.Combine(dir.FullName, "PadelClub.Console", "appsettings.json");
            if (File.Exists(candidato))
                return candidato;
            dir = dir.Parent;
        }

        throw new InvalidOperationException("No se encontró PadelClub.Console/appsettings.json.");
    }
}
