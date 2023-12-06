using Examenes.Models;
using Examenes.ViewModels;

namespace  Examenes.Services;

    public interface IAddressService
    {
        Task<Address> GetAddressByIdAsync(int id);
        Task<List<AddressViewModel>> GetAddressesAsync();
        Task<List<AddressViewModel>> GetAddressesAsync(string filter);
        Task<AddressViewModel> GetAddressDetailsAsync(int id);
        Task<AddressCreateViewModel> GetCreateViewModelAsync(List<Client> clients);
        Task<bool> CreateAddressAsync(AddressCreateViewModel addressView);
        Task<AddressEditViewModel> GetEditViewModelAsync(int id);
        Task<bool> EditAddressAsync(int id, AddressEditViewModel addressView);
        Task<AddressDeleteViewModel> GetDeleteViewModelAsync(int id);
        Task<bool> DeleteAddressAsync(int id);
    }
