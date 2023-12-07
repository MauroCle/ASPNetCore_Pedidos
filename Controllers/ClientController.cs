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
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        // GET: Client
        public async Task<IActionResult> Index(string NameFilter)
        {
            var clients = await _clientService.GetAllClientsAsync(NameFilter);
            var model = new ClientViewModel { Clients = clients };
            return View(model);
        }

        // GET: Client/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
               return NotFound(); 
            }

            var client = await _clientService.GetClientByIdAsync(id.Value);
            var address = await _clientService.GetAddressByClientIdAsync(id.Value);

            if (client == null)
            {
                return NotFound(); 
            }

            var clientView = new ClientDetailsViewModel
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                Address = address
            };

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
            ModelState.Remove("Orders");
            if (ModelState.IsValid)
            {
                var client = new Client
                {
                    FirstName = clientView.FirstName,
                    LastName = clientView.LastName,
                    Email = clientView.Email,
                    PhoneNumber = clientView.PhoneNumber,
                    Orders = new List<Order>()
                };

                var address = new Address
                {
                    City = clientView.City,
                    Street = clientView.Street,
                    Number = clientView.Number,
                    Apartment = clientView.Apartment,
                    Notes = clientView.Notes,
                    PostalCode = clientView.PostalCode,
                };

                await _clientService.CreateClientAsync(client, address);

                return RedirectToAction(nameof(Index));
            }
            return View(clientView);
        }

        // GET: Client/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
               return NotFound(); 
            }

            var client = await _clientService.GetClientByIdAsync(id.Value);
            var address = await _clientService.GetAddressByClientIdAsync(id.Value);

            if (client == null)
            {
                return NotFound(); 
            }

            var clientView = new ClientDetailsViewModel
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                Address = address
            };

            return View(clientView);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PhoneNumber")] ClientDetailsViewModel client)
        {
            if (id != client.Id)
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var updatedClient = await _clientService.GetClientByIdAsync(client.Id);
                updatedClient.FirstName = client.FirstName;
                updatedClient.LastName = client.LastName;
                updatedClient.Email = client.Email;
                updatedClient.PhoneNumber = client.PhoneNumber;

                await _clientService.UpdateClientAsync(updatedClient);

                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Client/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound(); 
            }

            var client = await _clientService.GetClientByIdAsync(id.Value);
            var address = await _clientService.GetAddressByClientIdAsync(id.Value);

            if (client == null)
            {
                return NotFound(); 
            }

            var clientView = new ClientDeleteViewModel
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                Address = address
            };

            return View(clientView);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           
            var hasOrders = await _clientService.ClientHasOrdersAsync(id);
            if (hasOrders)
            {
                ModelState.AddModelError(string.Empty, "No se puede eliminar clientes con pedidos asociados.");
                ClientDeleteViewModel clientDelete = await _clientService.GetClientDeleteByIdAsync(id);

                return View(clientDelete);
            }

            var success = await _clientService.DeleteClientAsync(id);

            if (!success)
            {
                ClientDeleteViewModel clientDelete = await _clientService.GetClientDeleteByIdAsync(id);
                
                return View(clientDelete);
            }

            return RedirectToAction(nameof(Index));
        }

        // private bool ClientExists(int id)
        // {
        //   return (_context.Client?.Any(e => e.Id == id)).GetValueOrDefault();
        // }
    }
}
