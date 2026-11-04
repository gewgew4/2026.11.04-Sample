using PadelClub.Application.Abstracciones;
using PadelClub.Domain;
using PadelClub.Domain.Comun;
using PadelClub.Domain.Entidades;

using PadelClub.Domain.Repositorios;

namespace PadelClub.Application.Socios;

public sealed record SocioDto(int Id, string Dni, string Nombre, string Apellido, string Email);

public sealed class SocioServicio(ISocioRepositorio repositorio, IReservaRepositorio reservas, IUnidadDeTrabajo unidad)
{
    public async Task<Result<IReadOnlyList<SocioDto>>> ListarActivosAsync(CancellationToken ct = default)
    {
        var socios = await repositorio.ListarActivosAsync(ct);
        IReadOnlyList<SocioDto> lista = socios
            .Select(s => new SocioDto(s.Id, s.Dni, s.Nombre, s.Apellido, s.Email))
            .ToList();
        return Result.Éxito(lista);
    }

    public async Task<Result<SocioDto>> CrearAsync(string dni, string nombre, string apellido, string email, CancellationToken ct = default)
    {
        var creado = Socio.Crear(dni, nombre, apellido, email, DateTime.Now);
        if (creado.EsFallo)
            return Result.Fallo<SocioDto>(creado.Error);

        var socio = creado.Valor;
        // S2 y S4. Misma idea que el número de cancha: se validan acá y el índice único cubre la carrera.
        if (await repositorio.ExisteDniAsync(socio.Dni, ct))
            return Result.Fallo<SocioDto>(Errores.Socio.DniDuplicado);
        if (await repositorio.ExisteEmailAsync(socio.Email, exceptoId: 0, ct))
            return Result.Fallo<SocioDto>(Errores.Socio.EmailDuplicado);

        repositorio.Agregar(socio);
        await unidad.GuardarAsync(ct);
        return Result.Éxito(new SocioDto(socio.Id, socio.Dni, socio.Nombre, socio.Apellido, socio.Email));
    }

    public async Task<Result> ModificarAsync(string dni, string nombre, string apellido, string email, CancellationToken ct = default)
    {
        var socio = await repositorio.ObtenerPorDniAsync(dni, ct);
        if (socio is null)
            return Result.Fallo(Errores.Socio.NoEncontrado);

        var limpio = email.Trim();
        // El duplicado se mira antes de Modificar. Si se modificara primero y el email estuviera tomado,
        // la entidad trackeada quedaría cambiada aunque el caso de uso devuelva Fallo.
        if (await repositorio.ExisteEmailAsync(limpio, socio.Id, ct))
            return Result.Fallo(Errores.Socio.EmailDuplicado);

        var modificado = socio.Modificar(nombre, apellido, limpio);
        if (modificado.EsFallo)
            return modificado;

        await unidad.GuardarAsync(ct);
        return Result.Éxito();
    }

    public async Task<Result> DarDeBajaAsync(string dni, CancellationToken ct = default)
    {
        var socio = await repositorio.ObtenerPorDniAsync(dni, ct);
        if (socio is null)
            return Result.Fallo(Errores.Socio.NoEncontrado);

        // S6, igual que C4: la entidad no ve reservas, así que la pregunta la hace el servicio.
        var ahora = DateTime.Now;
        if (await reservas.TieneActivasFuturasPorSocioAsync(socio.Id, ahora, ct))
            return Result.Fallo(Errores.Socio.TieneReservasFuturas);

        socio.DarDeBaja();
        await unidad.GuardarAsync(ct);
        return Result.Éxito();
    }
}
