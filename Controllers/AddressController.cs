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
    public class AddressController : Controller
    {
        private readonly YaPedidosContext _context;

        public AddressController(YaPedidosContext context)
        {
            _context = context;
        }

        // GET: Address
        public async Task<IActionResult> Index()
        {
            List<AddressViewModel> addressesView = new List<AddressViewModel>();
            var yaPedidosContext = await _context.Address.Include(a => a.Client).ToListAsync();

            foreach (var item in yaPedidosContext)
            {
                AddressViewModel addressView = new AddressViewModel()
                {
                    Id = item.Id,
                    City = item.City,
                    Street = item.Street,
                    Number = item.Number,
                    Apartment = item.Apartment,
                    Notes = item.Notes,
                    PostalCode = item.PostalCode,
                    ClientFirstName = item.Client.FirstName,
                    ClientLastName = item.Client.LastName,
                };
                addressesView.Add(addressView);
            }
            return View(addressesView);
        }


        // GET: Address/Details/5
        public async Task<IActionResult> Details(int? id)
        {


            if (id == null || _context.Address == null)
            {
                return NotFound();
            }

            var address = await _context.Address
                .Include(a => a.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }



            return View(address);
        }

        // GET: Address/Create
        public IActionResult Create()
        {
            var query = from Client in _context.Client select Client; 


            if(query.ToList().Count > 0) 
            {
                AddressCreateViewModel addressView = new AddressCreateViewModel();

                foreach (var item in query)
                {
                    addressView.Clients.Add(item);
                }
                return View(addressView);
            }
            

            return View();
        }

        // POST: Address/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,City,Street,Number,Apartment,Notes,PostalCode,ClientId")] AddressCreateViewModel addressView)
        {

            ModelState.Remove("Orders");
            if (ModelState.IsValid)
            {
            var address = new Address{
                City = addressView.City,
                Street = addressView.Street,
                Number = addressView.Number,
                Apartment = addressView.Apartment,
                Notes = addressView.Notes,
                PostalCode = addressView.PostalCode,
                ClientId = addressView.ClientId,
                };

                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", addressView.ClientId);
            return View(addressView);
        }

        // GET: Address/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Address == null)
            {
                return NotFound();
            }

            var address = await _context.Address.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
             var query = from Client in _context.Client select Client; 

            if(query.ToList().Count > 0) 
            {
                AddressEditViewModel addressView = new AddressEditViewModel()
                {
                Id = address.Id,
                City = address.City,
                Street = address.Street,
                Number = address.Number,
                Apartment = address.Apartment,
                Notes = address.Notes,
                PostalCode = address.PostalCode,
                ClientId = address.ClientId,
                };
                
                foreach (var item in query)
                {
                    addressView.Clients.Add(item);
                }
                return View(addressView);
            }

            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", address.ClientId);
            return View(address);
        }

        // POST: Address/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,City,Street,Number,Apartment,Notes,PostalCode,ClientId")] AddressEditViewModel addressView)
        {
            if (id != addressView.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingAddress = _context.Address.Find(addressView.Id);
                    if (existingAddress == null)
                    {
                        return NotFound();
                    }

                    _context.Entry(existingAddress).CurrentValues.SetValues(addressView);

                    _context.Update(existingAddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(addressView.Id))
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
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", addressView.ClientId);
            return View(addressView);
        }

        // GET: Address/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Address == null)
            {
                return NotFound();
            }

            var address = await _context.Address
                .Include(a => a.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (address == null)
            {
                return NotFound();
            }

            AddressDeleteViewModel addressView = new AddressDeleteViewModel(){
                Id = address.Id,
                City = address.City,
                Street = address.Street,
                Number = address.Number,
                Apartment = address.Apartment,
                Notes = address.Notes,
                PostalCode = address.PostalCode,
                ClientData = $"{address.Client.FirstName} {address.Client.LastName} - {address.Client.Email}"
            };


            return View(addressView);
        }

        // POST: Address/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Address == null)
            {
                return Problem("Entity set 'YaPedidosContext.Address'  is null.");
            }
            var address = await _context.Address.FindAsync(id);
            if (address != null)
            {
                _context.Address.Remove(address);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(int id)
        {
          return (_context.Address?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
