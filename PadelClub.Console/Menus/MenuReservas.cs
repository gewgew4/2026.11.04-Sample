using PadelClub.Application.Reservas;

namespace Consola.Menus;

// Paso concreto del Template Method. Pide los datos y muestra el Result que devuelve el servicio.
internal sealed class MenuReservas(ReservaServicio servicio) : MenuPlantilla
{
    protected override void MostrarOpciones()
    {
        Console.WriteLine("--- Reservas ---");
        Console.WriteLine(" 1. Nueva reserva");
        Console.WriteLine(" 2. Cancelar");
        Console.WriteLine(" 3. Agenda del día");
        Console.WriteLine(" 0. Volver");
    }

    protected override async Task EjecutarOpcionAsync(string opcion)
    {
        switch (opcion)
        {
            case "1": await NuevaAsync(); break;
            case "2": await CancelarAsync(); break;
            case "3": await AgendaAsync(); break;
            default: Console.WriteLine("Opción inválida."); break;
        }
    }

    async Task NuevaAsync()
    {
        var dni = Lector.LeerTexto("DNI del socio");
        var número = Lector.LeerEntero("Número de cancha");
        var inicio = Lector.LeerInicio();
        var resultado = await servicio.CrearAsync(dni, número, inicio);
        if (Salida.MostrarSiFallo(resultado))
            return;

        var reserva = resultado.Valor;
        Console.WriteLine(
            $"Reserva #{reserva.Id} creada. Cancha {reserva.NúmeroCancha}, {reserva.Inicio:dd/MM HH:mm} a {reserva.Fin:HH:mm}, {reserva.Importe:C0}.");
    }

    async Task CancelarAsync()
    {
        var id = Lector.LeerEntero("Id de reserva");
        var resultado = await servicio.CancelarAsync(id);
        if (Salida.MostrarSiFallo(resultado))
            return;
        Console.WriteLine($"Reserva #{id} cancelada.");
    }

    async Task AgendaAsync()
    {
        var día = Lector.LeerFecha("Fecha");
        var resultado = await servicio.AgendaAsync(día);
        if (Salida.MostrarSiFallo(resultado))
            return;
        if (resultado.Valor.Count == 0)
        {
            Console.WriteLine("No hay reservas activas.");
            return;
        }

        foreach (var turno in resultado.Valor)
            Console.WriteLine(
                $"Cancha {turno.NúmeroCancha}  {turno.Inicio:HH:mm} a {turno.Fin:HH:mm}  {turno.Socio}  #{turno.ReservaId}  {turno.Importe:C0}");
    }
}
