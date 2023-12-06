using Examenes.Models;

namespace  Examenes.Services;
    public interface IClientService
    {
        Task<List<Client>> GetAllClientsAsync(string nameFilter);
        Task<List<Client>> GetClientsWithoutAddressAsync();
        Task<Client> GetClientByIdAsync(int id);
        Task<Address> GetAddressByClientIdAsync(int clientId);
        Task<Client> CreateClientAsync(Client client);
        Task<Client> CreateClientAsync(Client client, Address address);
        Task<Address> CreateAddressAsync(Address address);
        Task<bool> DeleteClientAsync(int id);
        Task<Client> UpdateClientAsync(Client updatedClient);
    }
