using PadelClub.Domain.Entidades;

namespace PadelClub.Domain.Repositorios;

public interface ISocioRepositorio
{
    Task<IReadOnlyList<Socio>> ListarActivosAsync(CancellationToken ct = default);
    Task<Socio?> ObtenerPorDniAsync(string dni, CancellationToken ct = default);
    Task<bool> ExisteDniAsync(string dni, CancellationToken ct = default);
    Task<bool> ExisteEmailAsync(string email, int exceptoId, CancellationToken ct = default);
    void Agregar(Socio socio);
}
