using PadelClub.Domain.Entidades;

namespace PadelClub.Domain.Repositorios;

public interface IReservaRepositorio
{
    Task<Reserva?> ObtenerPorIdAsync(int id, CancellationToken ct = default);

    // R7. El filtro está en ReservaRepositorio y EF lo traduce a SQL.
    Task<Reserva?> BuscarSuperpuestaAsync(int canchaId, DateTime inicio, DateTime fin, CancellationToken ct = default);

    Task<bool> TieneActivasFuturasPorCanchaAsync(int canchaId, DateTime ahora, CancellationToken ct = default);
    Task<bool> TieneActivasFuturasPorSocioAsync(int socioId, DateTime ahora, CancellationToken ct = default);
    Task<IReadOnlyList<Reserva>> AgendaAsync(DateOnly día, CancellationToken ct = default);
    void Agregar(Reserva reserva);
}
