using PadelClub.Application.Abstracciones;
using PadelClub.Domain;
using PadelClub.Domain.Comun;
using PadelClub.Domain.Entidades;

using PadelClub.Domain.Repositorios;

namespace PadelClub.Application.Reservas;

public sealed record ReservaCreadaDto(int Id, int NúmeroCancha, DateTime Inicio, DateTime Fin, decimal Importe);
public sealed record TurnoAgendaDto(int ReservaId, int NúmeroCancha, string Socio, DateTime Inicio, DateTime Fin, decimal Importe);

public sealed class ReservaServicio(
    IReservaRepositorio reservas,
    ICanchaRepositorio canchas,
    ISocioRepositorio socios,
    IUnidadDeTrabajo unidad)
{
    public async Task<Result<ReservaCreadaDto>> CrearAsync(string dni, int númeroCancha, DateTime inicio, CancellationToken ct = default)
    {
        var ahora = DateTime.Now;

        // R6. Socio inexistente e inactivo comparten código: el enunciado no separa los dos casos.
        var socio = await socios.ObtenerPorDniAsync(dni.Trim(), ct);
        if (socio is null || !socio.Activo)
            return Result.Fallo<ReservaCreadaDto>(Errores.Reserva.SocioInactivo);

        // R5. Cancha inexistente o deshabilitada.
        var cancha = await canchas.ObtenerPorNúmeroAsync(númeroCancha, ct);
        if (cancha is null || !cancha.Habilitada)
            return Result.Fallo<ReservaCreadaDto>(Errores.Reserva.CanchaNoDisponible);

        // El importe que entra al factory es PrecioPorTurno de este momento.
        // La reserva guarda esa copia. Una modificación posterior de la cancha no la reescribe.
        var creada = Reserva.Crear(cancha.Id, socio.Id, inicio, cancha.PrecioPorTurno, ahora);
        if (creada.EsFallo)
            return Result.Fallo<ReservaCreadaDto>(creada.Error);

        var reserva = creada.Valor;
        // R7 después del factory, porque el intervalo a comparar usa el Fin calculado.
        var ocupada = await reservas.BuscarSuperpuestaAsync(cancha.Id, reserva.Inicio, reserva.Fin, ct);
        if (ocupada is not null)
            return Result.Fallo<ReservaCreadaDto>(Errores.Reserva.Superpuesta);

        reservas.Agregar(reserva);
        await unidad.GuardarAsync(ct);
        return Result.Éxito(new ReservaCreadaDto(reserva.Id, cancha.Número, reserva.Inicio, reserva.Fin, reserva.Importe));
    }

    public async Task<Result> CancelarAsync(int id, CancellationToken ct = default)
    {
        var reserva = await reservas.ObtenerPorIdAsync(id, ct);
        if (reserva is null)
            return Result.Fallo(Errores.Reserva.NoEncontrada);

        var cancelada = reserva.Cancelar(DateTime.Now);
        if (cancelada.EsFallo)
            return cancelada;

        await unidad.GuardarAsync(ct);
        return Result.Éxito();
    }

    public async Task<Result<IReadOnlyList<TurnoAgendaDto>>> AgendaAsync(DateOnly día, CancellationToken ct = default)
    {
        var turnos = await reservas.AgendaAsync(día, ct);
        // Importe sale de la reserva, no del precio actual de la cancha.
        IReadOnlyList<TurnoAgendaDto> lista = turnos
            .Select(r => new TurnoAgendaDto(r.Id, r.Cancha.Número, $"{r.Socio.Apellido}, {r.Socio.Nombre}", r.Inicio, r.Fin, r.Importe))
            .ToList();
        return Result.Éxito(lista);
    }
}
