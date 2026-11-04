using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelClub.Domain.Entidades;
using PadelClub.Domain.Enums;

namespace PadelClub.Infrastructure.Persistencia.Configuraciones;

// Fluent API. La entidad de Domain no lleva atributos de EF.
// Índice único en Número, Tipo guardado como string, precio decimal(10,2) y las 4 canchas de semilla.
public sealed class CanchaConfiguracion : IEntityTypeConfiguration<Cancha>
{
    public void Configure(EntityTypeBuilder<Cancha> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Número).IsRequired();
        builder.HasIndex(c => c.Número).IsUnique();
        builder.Property(c => c.Tipo).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(c => c.PrecioPorTurno).HasColumnType("decimal(10,2)");

        builder.HasData(
            new { Id = 1, Número = 1, Tipo = TipoCancha.Cristal, Techada = true, PrecioPorTurno = 18000m, Habilitada = true },
            new { Id = 2, Número = 2, Tipo = TipoCancha.Cristal, Techada = false, PrecioPorTurno = 15000m, Habilitada = true },
            new { Id = 3, Número = 3, Tipo = TipoCancha.Muro, Techada = true, PrecioPorTurno = 16000m, Habilitada = true },
            new { Id = 4, Número = 4, Tipo = TipoCancha.Muro, Techada = false, PrecioPorTurno = 14000m, Habilitada = true });
    }
}
