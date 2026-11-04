using PadelClub.Domain.Comun;

namespace PadelClub.Domain;

// Catálogo único. Entidades y servicios devuelven estos objetos.
// El string del código queda escrito una sola vez, igual al enunciado.
public static class Errores
{
    public static class Cancha
    {
        public static readonly Error NúmeroInválido =
            new("Cancha.NumeroInvalido", "El número de cancha debe estar entre 1 y 99.");

        public static readonly Error NúmeroDuplicado =
            new("Cancha.NumeroDuplicado", "Ya existe una cancha con ese número.");

        public static readonly Error PrecioInválido =
            new("Cancha.PrecioInvalido", "El precio por turno debe ser mayor a 0.");

        public static readonly Error TieneReservasFuturas =
            new("Cancha.TieneReservasFuturas", "No se puede deshabilitar: tiene reservas activas futuras.");

        public static readonly Error NoEncontrada =
            new("Cancha.NoEncontrada", "No existe esa cancha.");
    }

    public static class Socio
    {
        public static readonly Error DniInválido =
            new("Socio.DniInvalido", "El DNI debe tener 7 u 8 dígitos, solo números.");

        public static readonly Error DniDuplicado =
            new("Socio.DniDuplicado", "Ya existe un socio con ese DNI.");

        public static readonly Error EmailInválido =
            new("Socio.EmailInvalido", "El email no tiene un formato válido.");

        public static readonly Error EmailDuplicado =
            new("Socio.EmailDuplicado", "Ya existe un socio con ese email.");

        public static readonly Error NombreObligatorio =
            new("Socio.NombreObligatorio", "El nombre y el apellido son obligatorios, de hasta 60 caracteres.");

        public static readonly Error TieneReservasFuturas =
            new("Socio.TieneReservasFuturas", "No se puede dar de baja: tiene reservas activas futuras.");

        public static readonly Error NoEncontrado =
            new("Socio.NoEncontrado", "No existe ese socio.");
    }

    public static class Reserva
    {
        public static readonly Error HorarioInválido =
            new("Reserva.HorarioInvalido", "El turno tiene que empezar en punto o y media.");

        public static readonly Error FueraDeHorario =
            new("Reserva.FueraDeHorario", "El club abre a las 08:00 y el último turno empieza a las 21:30.");

        public static readonly Error FechaFueraDeRango =
            new("Reserva.FechaFueraDeRango", "No se puede reservar en el pasado ni con más de 14 días de anticipación.");

        public static readonly Error CanchaNoDisponible =
            new("Reserva.CanchaNoDisponible", "La cancha no existe o está deshabilitada.");

        public static readonly Error SocioInactivo =
            new("Reserva.SocioInactivo", "El socio no existe o no está activo.");

        public static readonly Error Superpuesta =
            new("Reserva.Superpuesta", "La cancha ya está reservada en ese horario.");

        public static readonly Error YaCancelada =
            new("Reserva.YaCancelada", "La reserva ya estaba cancelada.");

        public static readonly Error CancelaciónTardía =
            new("Reserva.CancelacionTardia", "Solo se puede cancelar con al menos 24 horas de anticipación.");

        public static readonly Error NoEncontrada =
            new("Reserva.NoEncontrada", "No existe esa reserva.");
    }
}
