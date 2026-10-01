using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Eventos.Data.Db;

namespace Eventos.Data.Inyecciones
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddDataAccess(
            this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<EventosContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}