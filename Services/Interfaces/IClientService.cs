using Examenes.Models;
using Examenes.ViewModels;

namespace  Examenes.Services;
    public interface IClientService
    {
        Task<List<Client>> GetAllClientsAsync(string nameFilter);
        Task<List<Client>> GetClientsWithoutAddressAsync();
        Task<Client> GetClientByIdAsync(int id);
        Task<Client> GetClientByIdWithAddressAsync(int id);
        Task<Address> GetAddressByClientIdAsync(int clientId);
        Task<Client> CreateClientAsync(Client client);
        Task<Client> CreateClientAsync(Client client, Address address);
        Task<Address> CreateAddressAsync(Address address);
        Task<bool> DeleteClientAsync(int id);
        Task<bool> ClientHasOrdersAsync(int id);
        Task<Client> UpdateClientAsync(Client updatedClient);
        Task<ClientDeleteViewModel> GetClientDeleteByIdAsync(int id);
    }
