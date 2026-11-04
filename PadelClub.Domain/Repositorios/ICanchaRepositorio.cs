using PadelClub.Domain.Entidades;

namespace PadelClub.Domain.Repositorios;

// Repository. El contrato vive en Domain y no menciona EF Core.
// Devuelve entidades o un bool. Nunca un IQueryable: la consulta se ejecuta en Infrastructure.
// El preparcial lo pide como arquitectura de base, aparte de los dos patrones a elegir.
public interface ICanchaRepositorio
{
    Task<IReadOnlyList<Cancha>> ListarAsync(CancellationToken ct = default);
    Task<Cancha?> ObtenerPorNúmeroAsync(int número, CancellationToken ct = default);
    Task<bool> ExisteNúmeroAsync(int número, CancellationToken ct = default);
    void Agregar(Cancha cancha);
}
