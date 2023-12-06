using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examenes.Data;
using Examenes.Models;
using Microsoft.EntityFrameworkCore;

namespace Examenes.Services
{
    public class ClientService : IClientService
    {
        private readonly YaPedidosContext _context;

        public ClientService(YaPedidosContext context)
        {
            _context = context;
        }

        public async Task<List<Client>> GetAllClientsAsync(string nameFilter)
        {
            var query = from client in _context.Client select client;

            if (!string.IsNullOrEmpty(nameFilter))
            {
                query = query.Where(x => x.FirstName.ToLower().Contains(nameFilter.ToLower()) ||
                                         x.LastName.ToLower().Contains(nameFilter.ToLower()) ||
                                         x.Email.ToLower().Contains(nameFilter.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<Client> GetClientByIdAsync(int id)
        {
            return await _context.Client.FindAsync(id);
        }

        public async Task<List<Address>> GetAddressesByClientIdAsync(int clientId)
        {
            return await _context.Address.Where(m => m.ClientId == clientId).ToListAsync();
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            _context.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<Client> CreateClientAsync(Client client, Address address)
        {
            _context.Add(client);
            await _context.SaveChangesAsync();

            address.ClientId = client.Id; 
            _context.Add(address);
            await _context.SaveChangesAsync();

            return client;
        }

        public async Task<Address> CreateAddressAsync(Address address)
        {
            _context.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            var client = await _context.Client.FindAsync(id);
            var addresses = await _context.Address.Where(m => m.ClientId == id).ToListAsync();

            if (client != null)
            {
                foreach (var item in addresses)
                {
                    _context.Address.Remove(item);
                }

                _context.Client.Remove(client);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<Client> UpdateClientAsync(Client updatedClient)
        {
            _context.Update(updatedClient);
            await _context.SaveChangesAsync();
            return updatedClient;
        }
    }
}