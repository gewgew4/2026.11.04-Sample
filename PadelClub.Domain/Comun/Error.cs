namespace PadelClub.Domain.Comun;

// Código es el texto estable del preparcial, por ejemplo Reserva.Superpuesta.
// Mensaje es la frase que ve el encargado. La consola imprime los dos.
public sealed record Error(string Código, string Mensaje)
{
    public static readonly Error Ninguno = new(string.Empty, string.Empty);
}
