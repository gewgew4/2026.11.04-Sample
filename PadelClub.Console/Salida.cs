using PadelClub.Domain.Comun;

namespace Consola;

// La consola decide el formato. El servicio solo entrega el Result.
internal static class Salida
{
    public static bool MostrarSiFallo(Result resultado)
    {
        if (resultado.EsÉxito)
            return false;

        Console.WriteLine($"[{resultado.Error.Código}] {resultado.Error.Mensaje}");
        return true;
    }
}
