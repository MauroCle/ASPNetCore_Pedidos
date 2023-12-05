using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Examenes.Data;
using Examenes.Models;
using Examenes.ViewModels;

namespace Examen.Controllers
{
    public class ProductController : Controller
    {
        private readonly YaPedidosContext _context;

        public ProductController(YaPedidosContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index(string NameFilter)
        {
            var query = from Product in _context.Product select Product;

            if(!string.IsNullOrEmpty(NameFilter))
            {
                query = query.Where(x => x.Name.ToLower().Contains(NameFilter.ToLower()) || x.Description.ToLower().Contains(NameFilter.ToLower()));
            }

            var model = new ProductIndexViewModel();

            if(query.Count() >0){
                foreach(var item in query){
                    ProductViewModel productView = new ProductViewModel(){
                        Id=item.Id,
                        Name=item.Name,
                        Description=item.Description,
                        Price=item.Price,
                        Stock=item.Stock
                    };
                    model.Products.Add(productView);
                }
            }

             return View(model);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            ProductViewModel productView = new ProductViewModel(){
                Id=product.Id,
                Name=product.Name,
                Description=product.Description,
                Price=product.Price,
                Stock=product.Stock
            };

            return View(productView);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Stock")] ProductViewModel productView)
        {
            ModelState.Remove("Orders");
            if (ModelState.IsValid)
            {
                Product product = new Product(){
                    Name=productView.Name,
                    Description=productView.Description,
                    Price=productView.Price,
                    Stock=productView.Stock
                };


                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productView);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            ProductViewModel productView = new ProductViewModel(){
                Id=product.Id,
                Name=product.Name,
                Description=product.Description,
                Price=product.Price,
                Stock=product.Stock
            };


            return View(productView);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Stock")] ProductViewModel productView)
        {
            if (id != productView.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = _context.Product.Find(productView.Id);

                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    _context.Entry(existingProduct).CurrentValues.SetValues(productView);

                    _context.Update(existingProduct);


                    _context.Update(existingProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(productView.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productView);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

        
            ProductViewModel productView = new ProductViewModel(){
            Id=product.Id,
            Name=product.Name,
            Description=product.Description,
            Price=product.Price,
            Stock=product.Stock
            };
            
            return View(productView);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'YaPedidosContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
