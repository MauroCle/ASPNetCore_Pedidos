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

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        // GET: Address
        public async Task<IActionResult> Index()
        {
            var addressesView = await _addressService.GetAddressesAsync();
        return View(addressesView);
        }


        // GET: Address/Details/5
        public async Task<IActionResult> Details(int? id)
        {
        if (id == null)
        {
            return NotFound();
        }

        var addressView = await _addressService.GetAddressDetailsAsync(id.Value);

        if (addressView == null)
        {
            return NotFound();
        }

        return View(addressView);
        }

        // GET: Address/Create
    public async Task<IActionResult> Create()
    {
        var addressView = await _addressService.GetCreateViewModelAsync();

        if (addressView == null)
        {
            // TODO Manejar el caso cuando no hay clientes disponibles
            return NotFound();
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
            return NotFound();
        }

        var addressView = await _addressService.GetEditViewModelAsync(id.Value);

            if (addressView == null)
            {
                return NotFound();
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
            return NotFound();
        }

        var addressView = await _addressService.GetDeleteViewModelAsync(id.Value);

        if (addressView == null)
        {
            return NotFound();
        }

        return View(addressView);
        }

        // POST: Address/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
        if (await _addressService.DeleteAddressAsync(id))
        {
            return RedirectToAction(nameof(Index));
        }

        // Handle deletion errors
        return RedirectToAction(nameof(Delete), new { id = id, error = true });
    
        }

        // private bool AddressExists(int id)
        // {
        //   return (_context.Address?.Any(e => e.Id == id)).GetValueOrDefault();
        // }
    }
}
