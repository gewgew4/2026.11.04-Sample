using Microsoft.EntityFrameworkCore;
using PadelClub.Domain.Entidades;

namespace PadelClub.Infrastructure.Persistencia;

// La migración está en Migrations. Para crear la base, desde la carpeta de la solución:
//   dotnet ef database update --project PadelClub.Infrastructure --startup-project PadelClub.Console
public sealed class PadelClubDbContext(DbContextOptions<PadelClubDbContext> opciones) : DbContext(opciones)
{
    public DbSet<Cancha> Canchas => Set<Cancha>();
    public DbSet<Socio> Socios => Set<Socio>();
    public DbSet<Reserva> Reservas => Set<Reserva>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PadelClubDbContext).Assembly);
}
