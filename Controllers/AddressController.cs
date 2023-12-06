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
    public class AddressController : Controller
    {
        // private readonly YaPedidosContext _context;

        // public AddressController(YaPedidosContext context)
        // {
        //     _context = context;
        // }
        private readonly IAddressService _addressService;
        private readonly IClientService _clientService;

        public AddressController(IAddressService addressService,IClientService clientService)
        {
            _addressService = addressService;
            _clientService = clientService;
        }

        // GET: Address
        public async Task<IActionResult> Index(string filter)
        {
            var addressesView = await _addressService.GetAddressesAsync(filter);
            var model = new AddressIndexViewModel { Addresses = addressesView };
            return View(model);
        }


        // GET: Address/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var addressView = await _addressService.GetAddressDetailsAsync(id.Value);

            if (addressView == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(addressView);
        }

        // GET: Address/Create
        public async Task<IActionResult> Create()
        {

            var clientsWithoutAddress = await _clientService.GetClientsWithoutAddressAsync();
            

            var addressView = await _addressService.GetCreateViewModelAsync(clientsWithoutAddress);
            
            if (addressView == null)
            {
                addressView = new AddressCreateViewModel(); // O inicializa según tus necesidades
            }

            return View(addressView);
        }

        // POST: Address/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,City,Street,Number,Apartment,Notes,PostalCode,ClientId")] AddressCreateViewModel addressView)
        {

            if (await _addressService.CreateAddressAsync(addressView))
            {
                return RedirectToAction(nameof(Index));
            }

            // Handle validation errors or other creation issues
            // ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", addressView.ClientId);
            return View(addressView);
        }

        // GET: Address/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var clientsWithoutAddress = await _clientService.GetClientsWithoutAddressAsync();
            
            var addressView = await _addressService.GetEditViewModelAsync(id.Value);

            if (addressView == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if(clientsWithoutAddress != null && clientsWithoutAddress.Any())
            {
                foreach(var item in clientsWithoutAddress){
                addressView.Clients.Add(item);
                }
            }

            return View(addressView);
        }

        // POST: Address/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,City,Street,Number,Apartment,Notes,PostalCode,ClientId")] AddressEditViewModel addressView)
        {
            if (await _addressService.EditAddressAsync(id, addressView))
            {
                return RedirectToAction(nameof(Index));
            }

            // Handle validation errors or other edit issues
            // ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", addressView.ClientId);
            return View(addressView);
        }

        // GET: Address/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var addressView = await _addressService.GetDeleteViewModelAsync(id.Value);

            if (addressView == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(addressView);
        }

        // POST: Address/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var address = await _addressService.GetAddressByIdAsync(id);


            if (await _clientService.ClientHasOrdersAsync(address.ClientId))
            {
                ModelState.AddModelError(string.Empty, "No se puede eliminar direcciones de clientes con pedidos asociados.");

                return View(await _addressService.GetDeleteViewModelAsync(id));
            }

            var success = await _addressService.DeleteAddressAsync(id);

            if (!success)
            { 
                return View(await _addressService.GetDeleteViewModelAsync(id));
            }

            return RedirectToAction(nameof(Index));

        }


    }
}
