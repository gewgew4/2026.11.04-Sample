namespace Consola.Menus;

// Template Method, el otro patrón elegido.
// EjecutarAsync fija el ciclo: mostrar, leer, volver con 0, ejecutar la opción y pausar.
// MostrarOpciones y EjecutarOpcionAsync son los pasos que escribe cada submenú.
// No hay try/catch. Una falla técnica sube al while de Program y corta el submenú.
internal abstract class MenuPlantilla
{
    public async Task EjecutarAsync()
    {
        while (true)
        {
            Console.WriteLine();
            MostrarOpciones();
            var opcion = Lector.LeerTexto("Opción");
            if (opcion == "0")
                return;

            await EjecutarOpcionAsync(opcion);
            Console.WriteLine();
            Console.WriteLine("Enter para continuar.");
            Console.ReadLine();
        }
    }

    protected abstract void MostrarOpciones();
    protected abstract Task EjecutarOpcionAsync(string opcion);
}
