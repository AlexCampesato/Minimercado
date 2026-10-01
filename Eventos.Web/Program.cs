using Eventos.Data.Db;
using Eventos.Web.Services;
using Microsoft.EntityFrameworkCore;
using Eventos.Data.Inyecciones;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("EventosDb");
builder.Services.AddDataAccess(connectionString);


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<EventosContext>();

    // Si no hay categorías, las creamos automáticamente
    if (!context.Categorias.Any())
    {
        context.Categorias.AddRange(
            new Eventos.Data.Modelos.Categoria { Nombre = "Bebidas" },
            new Eventos.Data.Modelos.Categoria { Nombre = "Almacén" },
            new Eventos.Data.Modelos.Categoria { Nombre = "Limpieza" },
            new Eventos.Data.Modelos.Categoria { Nombre = "Lácteos" },
            new Eventos.Data.Modelos.Categoria { Nombre = "Fiambrería" }
        );
        context.SaveChanges();
    }
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Productos}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
