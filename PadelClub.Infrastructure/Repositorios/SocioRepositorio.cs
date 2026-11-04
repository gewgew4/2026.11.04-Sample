using Microsoft.EntityFrameworkCore;
using PadelClub.Domain.Entidades;
using PadelClub.Domain.Repositorios;
using PadelClub.Infrastructure.Persistencia;

namespace PadelClub.Infrastructure.Repositorios;

public sealed class SocioRepositorio(PadelClubDbContext db) : ISocioRepositorio
{
    public async Task<IReadOnlyList<Socio>> ListarActivosAsync(CancellationToken ct = default) =>
        await db.Socios.AsNoTracking().Where(s => s.Activo).OrderBy(s => s.Apellido).ThenBy(s => s.Nombre).ToListAsync(ct);

    public Task<Socio?> ObtenerPorDniAsync(string dni, CancellationToken ct = default) =>
        db.Socios.FirstOrDefaultAsync(s => s.Dni == dni, ct);

    public Task<bool> ExisteDniAsync(string dni, CancellationToken ct = default) =>
        db.Socios.AnyAsync(s => s.Dni == dni, ct);

    // exceptoId deja pasar el email del propio socio en una modificación.
    // ToLower alinea la consulta con el índice único de SQL Server, que no distingue mayúsculas.
    public Task<bool> ExisteEmailAsync(string email, int exceptoId, CancellationToken ct = default) =>
        db.Socios.AnyAsync(s => s.Id != exceptoId && s.Email.ToLower() == email.ToLower(), ct);

    public void Agregar(Socio socio) => db.Socios.Add(socio);
}
