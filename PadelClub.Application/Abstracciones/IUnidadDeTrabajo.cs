namespace PadelClub.Application.Abstracciones;

// Unit of Work. El servicio modifica entidades trackeadas y llama GuardarAsync una vez.
// La interfaz está en Application para que el caso de uso no conozca el DbContext.
public interface IUnidadDeTrabajo
{
    Task GuardarAsync(CancellationToken ct = default);
}
