using PadelClub.Domain;
using PadelClub.Domain.Comun;
using PadelClub.Domain.Enums;


namespace PadelClub.Domain.Entidades;

public sealed class Reserva
{
    public static readonly TimeSpan Duración = TimeSpan.FromMinutes(90);
    public static readonly TimeOnly Apertura = new(8, 0);
    public static readonly TimeOnly ÚltimoInicio = new(21, 30);
    public const int DíasMáximos = 14;

    Reserva() { }

    public int Id { get; private set; }
    public int CanchaId { get; private set; }
    public int SocioId { get; private set; }
    public DateTime Inicio { get; private set; }
    public DateTime Fin { get; private set; }
    public decimal Importe { get; private set; }
    public EstadoReserva Estado { get; private set; }

    // Navegaciones para la agenda. EF las completa al leer. Crear no las usa.
    public Cancha Cancha { get; private set; } = null!;
    public Socio Socio { get; private set; } = null!;

    // Factory Method, uno de los dos patrones elegidos. La reserva solo nace por acá.
    // R1: Fin no se recibe. Se calcula como Inicio más 90 minutos.
    // R2: el minuto tiene que ser 00 o 30, con segundos en cero.
    // R3: la hora cae entre las 08:00 y las 21:30, que es el último inicio que termina a las 23:00.
    // R4: Inicio no puede ser anterior a ahora ni posterior a ahora más 14 días.
    // Importe entra ya decidido por el servicio y se copia. Cambiar después el precio de la cancha no toca esta fila.
    public static Result<Reserva> Crear(int canchaId, int socioId, DateTime inicio, decimal importe, DateTime ahora)
    {
        if (inicio.Minute is not (0 or 30) || inicio.Second != 0)
            return Errores.Reserva.HorarioInválido;

        var hora = TimeOnly.FromDateTime(inicio);
        if (hora < Apertura || hora > ÚltimoInicio)
            return Errores.Reserva.FueraDeHorario;

        if (inicio < ahora || inicio > ahora.AddDays(DíasMáximos))
            return Errores.Reserva.FechaFueraDeRango;

        return new Reserva
        {
            CanchaId = canchaId,
            SocioId = socioId,
            Inicio = inicio,
            Fin = inicio.Add(Duración),
            Importe = importe,
            Estado = EstadoReserva.Activa
        };
    }

    // El servicio no asigna Estado. Pide Cancelar y persiste si el Result es éxito.
    // R8 va primero: si ya está cancelada, la respuesta es YaCancelada aunque falten menos de 24 horas.
    // R9: se puede cancelar cuando falta al menos 24 horas. Igual a 24 horas entra.
    public Result Cancelar(DateTime ahora)
    {
        if (Estado == EstadoReserva.Cancelada)
            return Result.Fallo(Errores.Reserva.YaCancelada);

        if (Inicio - ahora < TimeSpan.FromHours(24))
            return Result.Fallo(Errores.Reserva.CancelaciónTardía);

        Estado = EstadoReserva.Cancelada;
        return Result.Éxito();
    }
}
