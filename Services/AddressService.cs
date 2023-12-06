using System.ComponentModel.DataAnnotations;
using Examenes.Data;
using Examenes.Models;
using Examenes.ViewModels;
using Microsoft.EntityFrameworkCore;
public class AddressService : IAddressService
{
    private readonly YaPedidosContext _context;

    public AddressService(YaPedidosContext context)
    {
        _context = context;
    }

   public async Task<List<AddressViewModel>> GetAddressesAsync()
    {
        var addresses = await _context.Address.Include(a => a.Client).ToListAsync();

        List<AddressViewModel> addressesView = addresses.Select(item => new AddressViewModel
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
        }).ToList();

        return addressesView;
    }

    public async Task<AddressViewModel> GetAddressDetailsAsync(int id)
    {
        var address = await _context.Address
            .Include(a => a.Client)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (address == null)
        {
            // TODO Manejar el caso cuando la dirección no se encuentra
            return null;
        }

        var addressView = new AddressViewModel
        {
            Id = address.Id,
            City = address.City,
            Street = address.Street,
            Number = address.Number,
            Apartment = address.Apartment,
            Notes = address.Notes,
            PostalCode = address.PostalCode,
            ClientFirstName = address.Client.FirstName,
            ClientLastName = address.Client.LastName,
        };

        return addressView;
    }

    public async Task<AddressCreateViewModel> GetCreateViewModelAsync()
    {
        var query = from client in _context.Client select client;

        if (query.Any())
        {
            var addressView = new AddressCreateViewModel();

            foreach (var item in query)
            {
                addressView.Clients.Add(item);
            }

            return addressView;
        }

        // TODO Manejar el caso cuando no hay clientes disponibles
        return null;
    }

  public async Task<bool> CreateAddressAsync(AddressCreateViewModel addressView)
    {
        var context = new ValidationContext(addressView, serviceProvider: null, items: null);
        var results = new List<ValidationResult>();

        if (Validator.TryValidateObject(addressView, context, results, validateAllProperties: true))
        {
            var address = new Address
            {
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
            return true;
        }

        return false;
    }

    public async Task<AddressEditViewModel> GetEditViewModelAsync(int id)
    {
        var address = await _context.Address.FindAsync(id);

        if (address == null)
        {
            // TODO Manejar el caso cuando la dirección no se encuentra
            return null;
        }

        var query = from client in _context.Client select client;

        if (query.Any())
        {
            var addressView = new AddressEditViewModel
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

            return addressView;
        }

        // TODO Manejar el caso cuando no hay clientes disponibles
        return null;
    }

   public async Task<bool> EditAddressAsync(int id, AddressEditViewModel addressView)
    {
        var context = new ValidationContext(addressView, serviceProvider: null, items: null);
        var results = new List<ValidationResult>();

        if (Validator.TryValidateObject(addressView, context, results, validateAllProperties: true))
        {
            try
            {
                var existingAddress = await _context.Address.FindAsync(addressView.Id);
                if (existingAddress == null)
                {
                    return false;
                }

                _context.Entry(existingAddress).CurrentValues.SetValues(addressView);

                _context.Update(existingAddress);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        return false;
    }
  public async Task<AddressDeleteViewModel> GetDeleteViewModelAsync(int id)
    {
        var address = await _context.Address
            .Include(a => a.Client)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (address == null)
        {
            // TODO Manejar el caso cuando la dirección no se encuentra
            return null;
        }

        var addressView = new AddressDeleteViewModel
        {
            Id = address.Id,
            City = address.City,
            Street = address.Street,
            Number = address.Number,
            Apartment = address.Apartment,
            Notes = address.Notes,
            PostalCode = address.PostalCode,
            ClientData = $"{address.Client.FirstName} {address.Client.LastName} - {address.Client.Email}"
        };

        return addressView;
    }

    public async Task<bool> DeleteAddressAsync(int id)
    {
        var address = await _context.Address.FindAsync(id);

        if (address == null)
        {
            //  TODO Manejar el caso cuando la dirección no se encuentra
            return false;
        }

        try
        {
            _context.Address.Remove(address);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            // TODO Manejar la excepción si ocurre algún problema al eliminar la dirección
            return false;
        }
    }
}