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
using Examenes.Services;
using Microsoft.AspNetCore.Authorization;

namespace Examen.Controllers
{    
    [Authorize(Roles = "Administrador,Comercial")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IClientService _clientService;
        private readonly IProductService _productService;

        public OrderController(IOrderService orderService, IClientService clientService, IProductService productService)
        {
            _orderService = orderService;
            _clientService = clientService;
            _productService = productService;
        }

        // GET: Order
        public async Task<IActionResult> Index(string NameFilter)
        {
            var orders = await _orderService.GetOrdersAsync(NameFilter);
            var model = new OrderIndexViewModel { Orders = orders };
            return View(model);
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); 
            }

            var orderDetail = await _orderService.GetOrderDetailsAsync(id.Value);

            if (orderDetail == null)
            {
                return NotFound(); 
            }

            return View(orderDetail);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new OrderCreateViewModel
            {
                OrderDate = DateTime.Today,
                Clients = await _clientService.GetAllClientsAsync(""), 
                Products = await _productService.GetAvalibleProductsAsync() 
            };

            ViewData["clients"] = await _clientService.GetAllClientsAsync("");
            return View(viewModel);
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateViewModel viewModel)
        {

            var clientsWithoutAddress = await _clientService.GetClientsWithoutAddressAsync();

            foreach(var item in clientsWithoutAddress)
            {   
                if(viewModel.ClientId == item.Id)
                {
                    ModelState.AddModelError(string.Empty, "El cliente debe tener una dirección asociada.");
                    
                    return View(viewModel);                      
                }
            }
            var result = await _orderService.CreateOrderAsync(viewModel);

            if (result)
            {   
                _productService.ReduceStockProducts(viewModel.ProductIds);
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(); 
            }

            var orderEdit = await _orderService.GetOrderEditViewModelAsync(id.Value);

            if (orderEdit == null)
            {
                return NotFound(); 
            }

             ViewData["clients"] = _clientService.GetAllClientsAsync("");
             ViewData["products"] = _productService.GetAvalibleProductsAsync();

            return View(orderEdit);
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,ProductIds,ClientId,Clients,Products")] OrderEditViewModel orderView)
        {
            if (id != orderView.Id)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.Remove("Clients");
            ModelState.Remove("Products");

            if (ModelState.IsValid)
            {
                var result = await _orderService.EditOrderAsync(id, orderView);

                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(orderView);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound(); 
            }

            var order = await _orderService.GetOrderDetailsAsync(id.Value);

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
            var result = await _orderService.DeleteOrderAsync(id);

            if (result)
            {
                return RedirectToAction(nameof(Index));
            }

            return Problem("Error deleting the order.");
        }
    }
}
