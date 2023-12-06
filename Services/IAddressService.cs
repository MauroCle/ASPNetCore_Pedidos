using Examenes.ViewModels;

public interface IAddressService
{
    Task<List<AddressViewModel>> GetAddressesAsync();
    Task<AddressViewModel> GetAddressDetailsAsync(int id);
    Task<AddressCreateViewModel> GetCreateViewModelAsync();
    Task<bool> CreateAddressAsync(AddressCreateViewModel addressView);
    Task<AddressEditViewModel> GetEditViewModelAsync(int id);
    Task<bool> EditAddressAsync(int id, AddressEditViewModel addressView);
    Task<AddressDeleteViewModel> GetDeleteViewModelAsync(int id);
    Task<bool> DeleteAddressAsync(int id);
}