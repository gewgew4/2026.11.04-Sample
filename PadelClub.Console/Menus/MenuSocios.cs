using PadelClub.Application.Socios;

namespace Consola.Menus;

// Paso concreto del Template Method. El ciclo de mostrar y pausar está en MenuPlantilla.
internal sealed class MenuSocios(SocioServicio servicio) : MenuPlantilla
{
    protected override void MostrarOpciones()
    {
        Console.WriteLine("--- Socios ---");
        Console.WriteLine(" 1. Listar activos");
        Console.WriteLine(" 2. Alta");
        Console.WriteLine(" 3. Modificar");
        Console.WriteLine(" 4. Dar de baja");
        Console.WriteLine(" 0. Volver");
    }

    protected override async Task EjecutarOpcionAsync(string opcion)
    {
        switch (opcion)
        {
            case "1": await ListarAsync(); break;
            case "2": await AltaAsync(); break;
            case "3": await ModificarAsync(); break;
            case "4": await BajaAsync(); break;
            default: Console.WriteLine("Opción inválida."); break;
        }
    }

    async Task ListarAsync()
    {
        var resultado = await servicio.ListarActivosAsync();
        if (Salida.MostrarSiFallo(resultado))
            return;
        if (resultado.Valor.Count == 0)
        {
            Console.WriteLine("No hay socios activos.");
            return;
        }

        foreach (var socio in resultado.Valor)
            Console.WriteLine($"{socio.Dni}  {socio.Apellido}, {socio.Nombre}  {socio.Email}");
    }

    async Task AltaAsync()
    {
        var dni = Lector.LeerTexto("DNI");
        var nombre = Lector.LeerTextoLibre("Nombre");
        var apellido = Lector.LeerTextoLibre("Apellido");
        var email = Lector.LeerTexto("Email");
        var resultado = await servicio.CrearAsync(dni, nombre, apellido, email);
        if (Salida.MostrarSiFallo(resultado))
            return;
        var socio = resultado.Valor;
        Console.WriteLine($"Socio {socio.Dni}, {socio.Nombre} {socio.Apellido}, dado de alta.");
    }

    async Task ModificarAsync()
    {
        var dni = Lector.LeerTexto("DNI");
        var nombre = Lector.LeerTextoLibre("Nombre");
        var apellido = Lector.LeerTextoLibre("Apellido");
        var email = Lector.LeerTexto("Email");
        var resultado = await servicio.ModificarAsync(dni, nombre, apellido, email);
        if (Salida.MostrarSiFallo(resultado))
            return;
        Console.WriteLine($"Socio {dni} actualizado.");
    }

    async Task BajaAsync()
    {
        var dni = Lector.LeerTexto("DNI");
        var resultado = await servicio.DarDeBajaAsync(dni);
        if (Salida.MostrarSiFallo(resultado))
            return;
        Console.WriteLine($"Socio {dni} dado de baja.");
    }
}
