using PadelClub.Application.Abstracciones;

namespace PadelClub.Infrastructure.Persistencia;

// Implementación del Unit of Work: un SaveChangesAsync abre la transacción de EF
// y escribe juntas las entidades que el caso de uso dejó modificadas.
public sealed class UnidadDeTrabajo(PadelClubDbContext db) : IUnidadDeTrabajo
{
    public Task GuardarAsync(CancellationToken ct = default) => db.SaveChangesAsync(ct);
}
