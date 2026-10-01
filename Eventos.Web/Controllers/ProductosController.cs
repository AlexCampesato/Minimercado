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

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos.Include(p => p.Categoria).ToListAsync();
            return View(productos);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre");
            return View();
        }

        // POST: Productos/Create
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

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            // Cargamos las categorías para el desplegable
            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre", producto.IdCategoria);
            return View(producto);
        }

        // POST: Productos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Producto productoViewModel)
        {
            if (id != productoViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Buscamos el producto real en la base de datos para no pisar campos vacíos
                    var productoOriginal = await _context.Productos.FindAsync(id);

                    if (productoOriginal == null)
                    {
                        return NotFound();
                    }

                    // Actualizamos únicamente los campos editables
                    productoOriginal.Nombre = productoViewModel.Nombre;
                    productoOriginal.Precio = productoViewModel.Precio;
                    productoOriginal.PesoGramos = productoViewModel.PesoGramos;
                    productoOriginal.IdCategoria = productoViewModel.IdCategoria;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Productos.Any(e => e.Id == productoViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // Redirección limpia al listado principal
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre", productoViewModel.IdCategoria);
            return View(productoViewModel);
        }
    }
}