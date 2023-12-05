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
    public class ClientController : Controller
    {
        private readonly YaPedidosContext _context;

        public ClientController(YaPedidosContext context)
        {
            _context = context;
        }

        // GET: Client
        public async Task<IActionResult> Index(string NameFilter)
        {
            var query = from Client in _context.Client select Client; //Genera un query de todo el menu

            //TODO Filtro por varios campos
            if(!string.IsNullOrEmpty(NameFilter)) //Si no trae valor name o no se especifica no entra en el if. Osea, trae todo el menu.
            {
                query = query.Where(x => x.FirstName.ToLower().Contains(NameFilter.ToLower())); //filtra la query anterior
            }
            var model = new ClientViewModel();
            model.Clients = query.ToList();

             return View(model);
        }

        // GET: Client/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var client = await _context.Client.FindAsync(id);
           var addresses = await _context.Address.Where(m => m.ClientId == id).ToListAsync();

            ClientDetailsViewModel clientView = new ClientDetailsViewModel(){
                Id= client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email= client.Email,
                PhoneNumber = client.PhoneNumber,
                Addresses = addresses
            };
            if (client == null || addresses == null)
            {
                return NotFound();
            }
            //TODO en la vista, revisar para que se muestren las direcciones
            return View(clientView);
        }

        // GET: Client/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Client/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PhoneNumber,City,Street,Number,Apartment,Notes,PostalCode")] ClientCreateViewModel clientView)
        {
            ModelState.Remove("Orders"); //TODO ver que hago con esto, creo que vuela
            if (ModelState.IsValid)
            {

                var client = new Client{
                FirstName = clientView.FirstName,
                LastName = clientView.LastName,
                Email = clientView.Email,
                PhoneNumber = clientView.PhoneNumber,
                Orders = new List<Order>()
                };


                _context.Add(client);
                await _context.SaveChangesAsync();

                var address = new Address{
                    City= clientView.City,
                    Street = clientView.Street,
                    Number = clientView.Number,
                    Apartment = clientView.Apartment,
                    Notes = clientView.Notes,
                    PostalCode = clientView.PostalCode,
                    ClientId = client.Id
                };
                
               // await AddressController.Create(address); TODO esto tendria que ser trabajo del service.


                return RedirectToAction(nameof(Index));
            }
            return View(clientView);
        }

        // GET: Client/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.Client == null || id==0)
            {
                return NotFound();
            }

            var client = await _context.Client.FindAsync(id);
           var addresses = await _context.Address.Where(m => m.ClientId == id).ToListAsync();

            ClientDetailsViewModel clientView = new ClientDetailsViewModel(){
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email= client.Email,
                PhoneNumber = client.PhoneNumber,
                Addresses = addresses
            };


            if (client == null || addresses == null) 
            {
                return NotFound();
            }
            return View(clientView);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PhoneNumber")] ClientDetailsViewModel client)
        {
            //TODO Aplicar el ViewModel
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            return View(client);
        }

        // GET: Client/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var client = await _context.Client.FindAsync(id);
           var addresses = await _context.Address.Where(m => m.ClientId == id).ToListAsync();

            ClientDeleteViewModel clientView = new ClientDeleteViewModel(){
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email= client.Email,
                PhoneNumber = client.PhoneNumber,
                Addresses = addresses
            };

            if (client == null||addresses ==null)
            {
                return NotFound();
            }

            return View(clientView);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //TODO Testar que las direcciones asociadas al client se eliminen correctamente
            if (_context.Client == null)
            {
                return Problem("Entity set 'YaPedidosContext.Client'  is null.");
            }
            var client = await _context.Client.FindAsync(id);
                var addresses = await _context.Address.Where(m => m.ClientId == id).ToListAsync();
            if (client != null)
            {   
                foreach(var item in addresses)
                {
                    _context.Address.Remove(item);
                }
                _context.Client.Remove(client);
                
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
          return (_context.Client?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
