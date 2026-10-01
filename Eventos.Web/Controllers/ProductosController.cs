using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eventos.Data.Db;
using Eventos.Data.Modelos;

namespace Eventos.Web.Controllers
{
    public class ProductosController : Controller
    {
        private readonly EventosContext _context;

        public ProductosController(EventosContext context)
        {
            _context = context;
        }

        // GET: Eventos (o Productos)
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos.Include(p => p.Categoria).ToListAsync();
            return View(productos);
        }

        // GET: Eventos/Create
        public IActionResult Create()
        {
            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre");
            return View();
        }

        // POST: Eventos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre", producto.IdCategoria);
            return View(producto);
        }
    }
}