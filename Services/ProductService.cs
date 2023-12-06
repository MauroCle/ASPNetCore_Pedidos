using Examenes.Data;
using Examenes.Models;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Examenes.ViewModels;
using Microsoft.EntityFrameworkCore;


namespace Examenes.Services
{

    public class ProductService : IProductService
    {
        private readonly YaPedidosContext _context;

        public ProductService(YaPedidosContext context)
        {
            _context = context;
        }

        public async Task<List<ProductViewModel>> GetProductsAsync(string nameFilter)
        {
            var query = _context.Product.AsQueryable();

            if (!string.IsNullOrEmpty(nameFilter))
            {
                query = query.Where(x => x.Name.ToLower().Contains(nameFilter.ToLower()) || x.Description.ToLower().Contains(nameFilter.ToLower()));
            }

            var products = await query.ToListAsync();

            return products.Select(product => new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            }).ToList();
        }

        public async Task<ProductViewModel?> GetProductAsync(int id)
        {
            var product = await _context.Product.FindAsync(id);

            return product?.Id != null
                ? new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock
                }
                : null;
        }

        public async Task CreateProductAsync(ProductViewModel productViewModel)
        {
            var product = new Product
            {
                Name = productViewModel.Name,
                Description = productViewModel.Description,
                Price = productViewModel.Price,
                Stock = productViewModel.Stock
            };

            _context.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(ProductViewModel productViewModel)
        {
            var existingProduct = await _context.Product.FindAsync(productViewModel.Id);

            if (existingProduct == null)
            {
                // Manejar la situación donde el producto no existe
                return;
            }

            _context.Entry(existingProduct).CurrentValues.SetValues(productViewModel);

            _context.Update(existingProduct);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product != null)
            {
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}