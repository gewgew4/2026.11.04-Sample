using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelClub.Domain.Entidades;

namespace PadelClub.Infrastructure.Persistencia.Configuraciones;

// Índices únicos en Dni y Email, que son la red de S2 y S4 si dos altas pasan juntas.
// La semilla trae 5 socios. Pedro Sosa, DNI 20123456, queda inactivo para probar Reserva.SocioInactivo.
public sealed class SocioConfiguracion : IEntityTypeConfiguration<Socio>
{
    static readonly DateTime Alta = new(2026, 1, 15);

    public void Configure(EntityTypeBuilder<Socio> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Dni).HasMaxLength(8).IsRequired();
        builder.HasIndex(s => s.Dni).IsUnique();
        builder.Property(s => s.Nombre).HasMaxLength(60).IsRequired();
        builder.Property(s => s.Apellido).HasMaxLength(60).IsRequired();
        builder.Property(s => s.Email).HasMaxLength(120).IsRequired();
        builder.HasIndex(s => s.Email).IsUnique();

        builder.HasData(
            new { Id = 1, Dni = "30111222", Nombre = "Ana", Apellido = "Pérez", Email = "ana.perez@padelclub.test", FechaAlta = Alta, Activo = true },
            new { Id = 2, Dni = "28999111", Nombre = "Luis", Apellido = "Gómez", Email = "luis.gomez@padelclub.test", FechaAlta = Alta, Activo = true },
            new { Id = 3, Dni = "33444555", Nombre = "María", Apellido = "Díaz", Email = "maria.diaz@padelclub.test", FechaAlta = Alta, Activo = true },
            new { Id = 4, Dni = "25666777", Nombre = "Juan", Apellido = "Ruiz", Email = "juan.ruiz@padelclub.test", FechaAlta = Alta, Activo = true },
            new { Id = 5, Dni = "20123456", Nombre = "Pedro", Apellido = "Sosa", Email = "pedro.sosa@padelclub.test", FechaAlta = Alta, Activo = false });
    }
}
