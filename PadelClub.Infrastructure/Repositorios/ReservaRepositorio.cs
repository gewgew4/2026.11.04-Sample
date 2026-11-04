using Microsoft.EntityFrameworkCore;
using PadelClub.Domain.Entidades;
using PadelClub.Domain.Enums;
using PadelClub.Domain.Repositorios;
using PadelClub.Infrastructure.Persistencia;

namespace PadelClub.Infrastructure.Repositorios;

public sealed class ReservaRepositorio(PadelClubDbContext db) : IReservaRepositorio
{
    public Task<Reserva?> ObtenerPorIdAsync(int id, CancellationToken ct = default) =>
        db.Reservas.FirstOrDefaultAsync(r => r.Id == id, ct);

    // R7. FirstOrDefaultAsync se traduce a SQL. No se carga la agenda en memoria.
    // Dos turnos se pisan cuando uno empieza antes de que el otro termine:
    // existente.Inicio < fin del nuevo, y el inicio nuevo < existente.Fin.
    // [10:00, 11:30) pisa [11:00, 12:30). No pisa [11:30, 13:00): 11:30 < 11:30 es falso,
    // así que el turno que arranca cuando el otro termina queda libre.
    // Solo cuentan las activas. Una cancelada deja el horario disponible.
    public Task<Reserva?> BuscarSuperpuestaAsync(int canchaId, DateTime inicio, DateTime fin, CancellationToken ct = default) =>
        db.Reservas.FirstOrDefaultAsync(r =>
            r.CanchaId == canchaId
            && r.Estado == EstadoReserva.Activa
            && r.Inicio < fin
            && inicio < r.Fin, ct);

    // C4 y S6. "Futura" es Inicio posterior a ahora: la reserva todavía no empezó.
    public Task<bool> TieneActivasFuturasPorCanchaAsync(int canchaId, DateTime ahora, CancellationToken ct = default) =>
        db.Reservas.AnyAsync(r => r.CanchaId == canchaId && r.Estado == EstadoReserva.Activa && r.Inicio > ahora, ct);

    public Task<bool> TieneActivasFuturasPorSocioAsync(int socioId, DateTime ahora, CancellationToken ct = default) =>
        db.Reservas.AnyAsync(r => r.SocioId == socioId && r.Estado == EstadoReserva.Activa && r.Inicio > ahora, ct);

    // Un solo SELECT con join, ordenado por cancha y hora, como pide el preparcial.
    // AsNoTracking porque la agenda no se edita. El servicio arma el DTO.
    public async Task<IReadOnlyList<Reserva>> AgendaAsync(DateOnly día, CancellationToken ct = default)
    {
        var desde = día.ToDateTime(TimeOnly.MinValue);
        var hasta = desde.AddDays(1);

        return await db.Reservas.AsNoTracking()
            .Include(r => r.Cancha)
            .Include(r => r.Socio)
            .Where(r => r.Estado == EstadoReserva.Activa && r.Inicio >= desde && r.Inicio < hasta)
            .OrderBy(r => r.Cancha.Número)
            .ThenBy(r => r.Inicio)
            .ToListAsync(ct);
    }

    public void Agregar(Reserva reserva) => db.Reservas.Add(reserva);
}
