using Microsoft.EntityFrameworkCore;
using Eventos.Data.Modelos;

namespace Eventos.Data.Db
{
    public class EventosContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public EventosContext(DbContextOptions<EventosContext> options) : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
    }
}