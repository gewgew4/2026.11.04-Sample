using PadelClub.Domain;
using PadelClub.Domain.Comun;
using PadelClub.Domain.Enums;


namespace PadelClub.Domain.Entidades;

// Setters privados. El alta entra por Crear y el precio por Modificar.
// El constructor sin parámetros lo usa EF Core para materializar la fila.
public sealed class Cancha
{
    Cancha() { }

    public int Id { get; private set; }
    public int Número { get; private set; }
    public TipoCancha Tipo { get; private set; }
    public bool Techada { get; private set; }
    public decimal PrecioPorTurno { get; private set; }
    public bool Habilitada { get; private set; }

    // Mismo Factory Method que Reserva.Crear. C1 y C3 viven en la entidad.
    // C2 (número repetido) y C4 (reservas futuras) necesitan otras tablas, así que quedan en el servicio.
    public static Result<Cancha> Crear(int número, TipoCancha tipo, bool techada, decimal precio)
    {
        if (número is < 1 or > 99)
            return Errores.Cancha.NúmeroInválido;
        if (precio <= 0)
            return Errores.Cancha.PrecioInválido;

        return new Cancha
        {
            Número = número,
            Tipo = tipo,
            Techada = techada,
            PrecioPorTurno = precio,
            Habilitada = true
        };
    }

    public Result Modificar(TipoCancha tipo, bool techada, decimal precio)
    {
        if (precio <= 0)
            return Result.Fallo(Errores.Cancha.PrecioInválido);

        Tipo = tipo;
        Techada = techada;
        PrecioPorTurno = precio;
        return Result.Éxito();
    }

    public void Deshabilitar() => Habilitada = false;
}
