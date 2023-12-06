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

namespace Examen.Controllers
{
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
                return RedirectToAction(nameof(Index));
            }

            var orderDetail = await _orderService.GetOrderDetailsAsync(id.Value);

            if (orderDetail == null)
            {
                return RedirectToAction(nameof(Index));
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

            return View(viewModel);
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateViewModel viewModel)
        {
            var result = await _orderService.CreateOrderAsync(viewModel);

            if (result)
            {
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var orderEdit = await _orderService.GetOrderEditViewModelAsync(id.Value);

            if (orderEdit == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // ViewData["ClientId"] = new SelectList(orderEdit.Clients, "Id", "Id", orderEdit.ClientId);
            // ViewData["AddressId"] = new SelectList(_context.Address, "Id", "Id", orderEdit.AddressId);

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

            // ViewData["ClientId"] = new SelectList(orderView.Clients, "Id", "Id", orderView.ClientId);
            // ViewData["AddressId"] = new SelectList(_context.Address, "Id", "Id", orderView.AddressId);

            return View(orderView);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var order = await _orderService.GetOrderDetailsAsync(id.Value);

            if (order == null)
            {
                return RedirectToAction(nameof(Index));
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
