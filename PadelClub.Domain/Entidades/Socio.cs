using PadelClub.Domain;
using PadelClub.Domain.Comun;


namespace PadelClub.Domain.Entidades;

public sealed class Socio
{
    Socio() { }

    public int Id { get; private set; }
    public string Dni { get; private set; } = "";
    public string Nombre { get; private set; } = "";
    public string Apellido { get; private set; } = "";
    public string Email { get; private set; } = "";
    public DateTime FechaAlta { get; private set; }
    public bool Activo { get; private set; }

    // Factory Method. S1, S3 y S5 se resuelven con el dato en la mano.
    // S2 y S4 comparan contra socios ya guardados, y S6 mira reservas: las dos cosas las hace SocioServicio.
    public static Result<Socio> Crear(string dni, string nombre, string apellido, string email, DateTime ahora)
    {
        if (!DniVálido(dni))
            return Errores.Socio.DniInválido;
        if (!TextoVálido(nombre) || !TextoVálido(apellido))
            return Errores.Socio.NombreObligatorio;
        if (!EmailVálido(email))
            return Errores.Socio.EmailInválido;

        return new Socio
        {
            Dni = dni.Trim(),
            Nombre = nombre.Trim(),
            Apellido = apellido.Trim(),
            Email = email.Trim(),
            FechaAlta = ahora,
            Activo = true
        };
    }

    public Result Modificar(string nombre, string apellido, string email)
    {
        if (!TextoVálido(nombre) || !TextoVálido(apellido))
            return Result.Fallo(Errores.Socio.NombreObligatorio);
        if (!EmailVálido(email))
            return Result.Fallo(Errores.Socio.EmailInválido);

        Nombre = nombre.Trim();
        Apellido = apellido.Trim();
        Email = email.Trim();
        return Result.Éxito();
    }

    public void DarDeBaja() => Activo = false;

    static bool DniVálido(string? dni) =>
        dni is not null && dni.Trim().Length is 7 or 8 && dni.Trim().All(char.IsDigit);

    static bool TextoVálido(string? texto) =>
        !string.IsNullOrWhiteSpace(texto) && texto.Trim().Length <= 60;

    static bool EmailVálido(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        var limpio = email.Trim();
        var arroba = limpio.IndexOf('@');
        var punto = limpio.LastIndexOf('.');
        return limpio.Length <= 120
            && arroba > 0
            && punto > arroba + 1
            && punto < limpio.Length - 1
            && !limpio.Contains(' ');
    }
}
