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
    public class OrderController : Controller
    {
        private readonly YaPedidosContext _context;

        public OrderController(YaPedidosContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index(string NameFilter)
        {
              var query = from Order in _context.Order select Order;

            if(!string.IsNullOrEmpty(NameFilter))
            {
                query = query.Where(x => x.Client.FirstName.ToLower().Contains(NameFilter.ToLower()) || 
                x.Client.LastName.ToLower().Contains(NameFilter.ToLower()) ||
                x.Client.Email.ToLower().Contains(NameFilter.ToLower()) ||
                x.ShippingAddress.City.ToLower().Contains(NameFilter.ToLower())||
                x.ShippingAddress.Street.ToLower().Contains(NameFilter.ToLower())||
                x.ShippingAddress.Number.ToString().Contains(NameFilter.ToLower())
                );
            }

            var model = query.Select(item => new OrderViewModel
                {
                    Id = item.Id,
                    OrderDate = item.OrderDate,
                    ShippingAddressData = $"{item.ShippingAddress.City} - {item.ShippingAddress.Street} {item.ShippingAddress.Number} ({item.ShippingAddress.PostalCode})",
                    ProductsQuantity = item.Products.Count(),
                    ClientData = $"{item.Client.FirstName} {item.Client.LastName} - {item.Client.Email}"
                }).ToList();
            

             return View(model);

        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Client)
                .Include(o => o.ShippingAddress)
                .Include(o => o.Products)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }
        
            OrderDetailViewModel OrderDetail = new OrderDetailViewModel(){
                    Id  = order.Id,
                    OrderDate  = order.OrderDate,  
                    ShippingAddressData  = $"{order.ShippingAddress.City} - {order.ShippingAddress.Street} {order.ShippingAddress.Number} ({order.ShippingAddress.PostalCode}) || {order.ShippingAddress.Notes}", 
                    Products = order.Products != null ? order.Products.ToList() : new List<Product>(), 
                    ClientData  =  $"{order.Client.FirstName} {order.Client.LastName} - ({order.Client.PhoneNumber}) || ({order.Client.Email})"

                };

            return View(OrderDetail);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            var clientsList = _context.Client.ToList();
            var productsList = _context.Product.ToList();

            var viewModel = new OrderCreateViewModel
            {
                OrderDate = DateTime.Today,
                Clients = clientsList,
                Products = productsList
            };

            return View(viewModel);
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Create(OrderCreateViewModel viewModel)
    {
        var clientsList = _context.Client.ToList();
        var productsList = _context.Product.ToList();
        List<Product> products = new List<Product>();
        ModelState.Remove("Products");
        ModelState.Remove("Clients");
        // TODO validaciones, view model null, id null, etc
        if (ModelState.IsValid)
        {
            Client clientOrder = _context.Client.Include(x => x.Address).Where(x => x.Id == viewModel.ClientId).FirstOrDefault();

            foreach(var item in viewModel.ProductIds){

                products.Add(productsList.FirstOrDefault(x => x.Id == item));
                
            }

            Order newOrder = new Order();

            newOrder.OrderDate = viewModel.OrderDate;
            newOrder.AddressId = clientOrder.Address.Id;
            newOrder.ShippingAddress = clientOrder.Address;
            newOrder.Products = products;
            newOrder.ClientId = clientOrder.Id;                    
           
            
            _context.Add(newOrder);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        viewModel.Clients = clientsList;
        viewModel.Products = productsList;

        return View(viewModel);
    }
    
        

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var clientsList = _context.Client.ToList();
            var productsList = _context.Product.ToList();


            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
            .Include(x => x.Products)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }






            OrderEditViewModel orderEdit = new OrderEditViewModel(){
                    Id  = order.Id,
                    OrderDate  = order.OrderDate,  
                    Products = productsList, 
                    ClientId  =  order.ClientId,
                    Clients = clientsList,
                    ProductIds = new List<int>()
            };

            if (order.Products != null && order.Products.Any())
            {
                foreach (var item in order.Products)
                {
                    if (item != null)
                        orderEdit.ProductIds.Add(item.Id);

                }
            }
                    
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", order.ClientId);
            ViewData["AddressId"] = new SelectList(_context.Address, "Id", "Id", order.AddressId);
            return View(orderEdit);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,ProductIds,ClientId,Clients,Products")] OrderEditViewModel orderView)
        {
            if (id != orderView.Id)
            {
                return NotFound();
            }
            
            ModelState.Remove("Clients");
            ModelState.Remove("Products");
            
            if (ModelState.IsValid)
            {
                try
                {
                    var order = await _context.Order
                    .Include(o => o.Products) 
                    .Include(o => o.Client)
                    .Include(o => o.ShippingAddress)  
                    .FirstOrDefaultAsync(m => m.Id == id);

                    if (order == null)
                    {
                        return NotFound();
                    }

                    order.OrderDate = orderView.OrderDate;
                    order.ClientId = orderView.ClientId;
                    order.Client = await _context.Client.FirstOrDefaultAsync(x => x.Id == orderView.ClientId);
                    order.ShippingAddress = await _context.Address.FirstOrDefaultAsync(x => x.ClientId == orderView.ClientId);
                    order.Products = await _context.Product.Where(p => orderView.ProductIds.Contains(p.Id)).ToListAsync();

                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(orderView.Id))
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
            // ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", orderView.ClientId);
            // ViewData["AddressId"] = new SelectList(_context.Address, "Id", "Id", orderView.AddressId);
            return View(orderView);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Client)
                .Include(o => o.ShippingAddress)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null)
            {
                return Problem("Entity set 'YaPedidosContext.Order'  is null.");
            }
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Order?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
