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
      private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        // GET: Product
        public async Task<IActionResult> Index(string NameFilter)
        {
            var model = new ProductIndexViewModel();
            model.Products = await _productService.GetProductsAsync(NameFilter);

            return View(model);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productView = await _productService.GetProductAsync(id.Value);

            if (productView == null)
            {
                return NotFound();
            }

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
            if (ModelState.IsValid)
            {
                await _productService.CreateProductAsync(productView);
                return RedirectToAction(nameof(Index));
            }
            return View(productView);
        }

        // GET: Product/Edit/5
          public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productView = await _productService.GetProductAsync(id.Value);

            if (productView == null)
            {
                return NotFound();
            }

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
            await _productService.UpdateProductAsync(productView);
            return RedirectToAction(nameof(Index));
        }
        return View(productView);
    }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productView = await _productService.GetProductAsync(id.Value);

        if (productView == null)
        {
            return NotFound();
        }

        return View(productView);
    }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _productService.DeleteProductAsync(id);
        return RedirectToAction(nameof(Index));
    }

        // private bool ProductExists(int id)
        // {
        //   return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        // }
    }
}
