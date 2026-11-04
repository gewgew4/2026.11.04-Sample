using PadelClub.Application.Canchas;

namespace Consola.Menus;

// Paso concreto del Template Method. Solo arma las opciones y llama a CanchaServicio.
internal sealed class MenuCanchas(CanchaServicio servicio) : MenuPlantilla
{
    protected override void MostrarOpciones()
    {
        Console.WriteLine("--- Canchas ---");
        Console.WriteLine(" 1. Listar");
        Console.WriteLine(" 2. Alta");
        Console.WriteLine(" 3. Modificar");
        Console.WriteLine(" 4. Deshabilitar");
        Console.WriteLine(" 0. Volver");
    }

    protected override async Task EjecutarOpcionAsync(string opcion)
    {
        switch (opcion)
        {
            case "1": await ListarAsync(); break;
            case "2": await AltaAsync(); break;
            case "3": await ModificarAsync(); break;
            case "4": await DeshabilitarAsync(); break;
            default: Console.WriteLine("Opción inválida."); break;
        }
    }

    async Task ListarAsync()
    {
        var resultado = await servicio.ListarAsync();
        if (Salida.MostrarSiFallo(resultado))
            return;
        if (resultado.Valor.Count == 0)
        {
            Console.WriteLine("No hay canchas.");
            return;
        }

        foreach (var cancha in resultado.Valor)
        {
            var techo = cancha.Techada ? "techada" : "al aire libre";
            var estado = cancha.Habilitada ? "habilitada" : "deshabilitada";
            Console.WriteLine($"#{cancha.Número}  {cancha.Tipo}  {techo}  {cancha.PrecioPorTurno:C0}  {estado}");
        }
    }

    async Task AltaAsync()
    {
        var número = Lector.LeerEntero("Número");
        var tipo = Lector.LeerTipo();
        var techada = Lector.LeerSíNo("Techada");
        var precio = Lector.LeerDecimal("Precio por turno");
        var resultado = await servicio.CrearAsync(número, tipo, techada, precio);
        if (Salida.MostrarSiFallo(resultado))
            return;
        Console.WriteLine($"Cancha #{resultado.Valor.Número} creada.");
    }

    async Task ModificarAsync()
    {
        var número = Lector.LeerEntero("Número");
        var tipo = Lector.LeerTipo();
        var techada = Lector.LeerSíNo("Techada");
        var precio = Lector.LeerDecimal("Precio por turno");
        var resultado = await servicio.ModificarAsync(número, tipo, techada, precio);
        if (Salida.MostrarSiFallo(resultado))
            return;
        Console.WriteLine($"Cancha #{número} actualizada.");
    }

    async Task DeshabilitarAsync()
    {
        var número = Lector.LeerEntero("Número");
        var resultado = await servicio.DeshabilitarAsync(número);
        if (Salida.MostrarSiFallo(resultado))
            return;
        Console.WriteLine($"Cancha #{número} deshabilitada.");
    }
}
