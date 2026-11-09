using Eventos.Data.Db;
using Eventos.Web.Services;
using Microsoft.EntityFrameworkCore;
using Eventos.Data.Inyecciones;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// 1. Validar que la cadena de conexión realmente se esté leyendo del appsettings.json
var connectionString = builder.Configuration.GetConnectionString("EventosDb")
    ?? throw new InvalidOperationException("No se encontró 'ConnectionStrings:EventosDb' en appsettings.json.");

builder.Services.AddDataAccess(connectionString);

// 2. Configurar Cookies (para guardar la sesión) + Google (para iniciar sesión)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;

    // Obliga a Google a mostrar el selector de cuentas cada vez que inicias sesión
    options.Events.OnRedirectToAuthorizationEndpoint = context =>
    {
        context.Response.Redirect(context.RedirectUri + "&prompt=select_account");
        return Task.CompletedTask;
    };
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Duración de la sesión del carrito
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<EventosContext>();

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
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Productos}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Productos}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();