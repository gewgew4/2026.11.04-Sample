using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PadelClub.Application.Abstracciones;
using PadelClub.Domain.Repositorios;
using PadelClub.Infrastructure.Persistencia;
using PadelClub.Infrastructure.Repositorios;

namespace PadelClub.Infrastructure;

// Composition root de Infrastructure. PadelClub.Console llama a esta extensión y no hace new de un repositorio.
// Cada interfaz de Domain queda unida a su clase de EF. El DbContext es scoped: uno por opción del menú.
public static class Inyeccion
{
    public static IServiceCollection AgregarInfraestructura(this IServiceCollection services, string cadena)
    {
        services.AddDbContext<PadelClubDbContext>(opciones => opciones.UseSqlServer(cadena));
        services.AddScoped<ICanchaRepositorio, CanchaRepositorio>();
        services.AddScoped<ISocioRepositorio, SocioRepositorio>();
        services.AddScoped<IReservaRepositorio, ReservaRepositorio>();
        services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();
        return services;
    }
}
