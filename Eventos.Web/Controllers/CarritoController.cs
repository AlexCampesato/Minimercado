using Microsoft.AspNetCore.Mvc;
using Eventos.Data.Db;
using Eventos.Web.Models;
using Eventos.Web.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Eventos.Web.Controllers
{
    [Authorize(Roles = "Cliente")] // Solo los clientes usan el carrito
    public class CarritoController : Controller
    {
        private readonly EventosContext _context;
        private const string SessionKeyCarrito = "CarritoCompras";

        public CarritoController(EventosContext context)
        {
            _context = context;
        }

        // Ver el contenido del carrito
        public IActionResult Index()
        {
            var carrito = HttpContext.Session.GetObjectFromJson<List<ItemCarrito>>(SessionKeyCarrito) ?? new List<ItemCarrito>();
            return View(carrito);
        }

        // Agregar un producto al carrito
        [HttpPost]
        public async Task<IActionResult> Agregar(int productoId)
        {
            var producto = await _context.Productos.FindAsync(productoId);
            if (producto == null) return NotFound();

            var carrito = HttpContext.Session.GetObjectFromJson<List<ItemCarrito>>(SessionKeyCarrito) ?? new List<ItemCarrito>();

            var itemExistente = carrito.FirstOrDefault(x => x.ProductoId == productoId);
            if (itemExistente != null)
            {
                itemExistente.Cantidad++;
            }
            else
            {
                carrito.Add(new ItemCarrito
                {
                    ProductoId = producto.Id,
                    Nombre = producto.Nombre,
                    Precio = (double)producto.Precio,
                    Cantidad = 1
                });
            }

            HttpContext.Session.SetObjectAsJson(SessionKeyCarrito, carrito);
            return RedirectToAction("Index", "Productos");
        }

        // Eliminar o reducir cantidad
        public IActionResult Remover(int productoId)
        {
            var carrito = HttpContext.Session.GetObjectFromJson<List<ItemCarrito>>(SessionKeyCarrito) ?? new List<ItemCarrito>();
            var item = carrito.FirstOrDefault(x => x.ProductoId == productoId);
            if (item != null)
            {
                carrito.Remove(item);
                HttpContext.Session.SetObjectAsJson(SessionKeyCarrito, carrito);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}