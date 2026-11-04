using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelClub.Domain.Entidades;

namespace PadelClub.Infrastructure.Persistencia.Configuraciones;

// Importe queda decimal(10,2) y Estado se guarda como string (Activa o Cancelada).
// Restrict: borrar una cancha o un socio no arrastra sus reservas.
public sealed class ReservaConfiguracion : IEntityTypeConfiguration<Reserva>
{
    public void Configure(EntityTypeBuilder<Reserva> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Inicio).IsRequired();
        builder.Property(r => r.Fin).IsRequired();
        builder.Property(r => r.Importe).HasColumnType("decimal(10,2)");
        builder.Property(r => r.Estado).HasConversion<string>().HasMaxLength(20).IsRequired();

        builder.HasOne(r => r.Cancha).WithMany().HasForeignKey(r => r.CanchaId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(r => r.Socio).WithMany().HasForeignKey(r => r.SocioId).OnDelete(DeleteBehavior.Restrict);
    }
}
