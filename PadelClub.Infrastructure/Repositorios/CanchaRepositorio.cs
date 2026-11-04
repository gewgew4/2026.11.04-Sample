using Microsoft.EntityFrameworkCore;
using PadelClub.Domain.Entidades;
using PadelClub.Domain.Repositorios;
using PadelClub.Infrastructure.Persistencia;

namespace PadelClub.Infrastructure.Repositorios;

// Implementación del Repository con EF Core.
// Listar no trackea: es una lectura. Obtener sí trackea, porque Modificar y Deshabilitar
// cambian la misma instancia y el Unit of Work la guarda con SaveChanges.
public sealed class CanchaRepositorio(PadelClubDbContext db) : ICanchaRepositorio
{
    public async Task<IReadOnlyList<Cancha>> ListarAsync(CancellationToken ct = default) =>
        await db.Canchas.AsNoTracking().OrderBy(c => c.Número).ToListAsync(ct);

    public Task<Cancha?> ObtenerPorNúmeroAsync(int número, CancellationToken ct = default) =>
        db.Canchas.FirstOrDefaultAsync(c => c.Número == número, ct);

    public Task<bool> ExisteNúmeroAsync(int número, CancellationToken ct = default) =>
        db.Canchas.AnyAsync(c => c.Número == número, ct);

    public void Agregar(Cancha cancha) => db.Canchas.Add(cancha);
}
