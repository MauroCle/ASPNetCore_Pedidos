using System.ComponentModel.DataAnnotations;
using Examenes.Data;
using Examenes.Models;
using Examenes.ViewModels;
using Microsoft.EntityFrameworkCore;
using Examenes.Services;

   public class OrderService : IOrderService
{
    private readonly YaPedidosContext _context;

    public OrderService(YaPedidosContext context)
    {
        _context = context;
    }

    public async Task<List<OrderViewModel>> GetOrdersAsync(string nameFilter)
    {
        var query = _context.Order.AsQueryable();

        if (!string.IsNullOrEmpty(nameFilter))
        {
            query = query.Where(x =>
                x.Client.FirstName.ToLower().Contains(nameFilter.ToLower()) ||
                x.Client.LastName.ToLower().Contains(nameFilter.ToLower()) ||
                x.Client.Email.ToLower().Contains(nameFilter.ToLower()) ||
                x.ShippingAddress.City.ToLower().Contains(nameFilter.ToLower()) ||
                x.ShippingAddress.Street.ToLower().Contains(nameFilter.ToLower()) ||
                x.ShippingAddress.Number.ToString().Contains(nameFilter.ToLower())
            );
        }

        var orders = await query
            .Select(item => new OrderViewModel
            {
                Id = item.Id,
                OrderDate = item.OrderDate,
                ShippingAddressData = $"{item.ShippingAddress.City} - {item.ShippingAddress.Street} {item.ShippingAddress.Number} ({item.ShippingAddress.PostalCode})",
                ProductsQuantity = item.Products.Count(),
                ClientData = $"{item.Client.FirstName} {item.Client.LastName} - {item.Client.Email}"
            })
            .ToListAsync();

        return orders;
    }

    public async Task<OrderDetailViewModel> GetOrderDetailsAsync(int id)
    {
        var order = await _context.Order
            .Include(o => o.Client)
            .Include(o => o.ShippingAddress)
            .Include(o => o.Products)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (order == null)
        {
            return null;
        }

        var orderDetail = new OrderDetailViewModel
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            ShippingAddressData = $"{order.ShippingAddress.City} - {order.ShippingAddress.Street} {order.ShippingAddress.Number} ({order.ShippingAddress.PostalCode}) || {order.ShippingAddress.Notes}",
            Products = order.Products != null ? order.Products.ToList() : new List<Product>(),
            ClientData = $"{order.Client.FirstName} {order.Client.LastName} - ({order.Client.PhoneNumber}) || ({order.Client.Email})"
        };

        return orderDetail;
    }
    public async Task<List<Order>> GetOrdersWithProductsAsync(){
        
        List<Order> orders = await _context.Order.Include(o => o.Products).ToListAsync();

        if (orders == null)
            return orders = new List<Order>();
        else
            return orders;
    }
    public async Task<bool> CreateOrderAsync(OrderCreateViewModel viewModel)
    {
        var clientsList = await _context.Client.Include(x=> x.Address).ToListAsync();
        var productsList = await _context.Product.ToListAsync();
        var products = new List<Product>();

        var validationContext = new ValidationContext(viewModel, serviceProvider: null, items: null);
        var validationResults = new List<ValidationResult>();

        if (Validator.TryValidateObject(viewModel, validationContext, validationResults, validateAllProperties: true))
        {
            var clientOrder = clientsList.FirstOrDefault(x => x.Id == viewModel.ClientId);

            if (clientOrder != null)
            {
                foreach (var productId in viewModel.ProductIds)
                {
                    var product = productsList.FirstOrDefault(x => x.Id == productId);
                    if (product != null)
                    {
                        products.Add(product);
                    }
                }

                Order newOrder = new Order
                {
                    OrderDate = viewModel.OrderDate,
                    AddressId = clientOrder.Address.Id,
                    ShippingAddress = clientOrder.Address,
                    Products = products,
                    ClientId = clientOrder.Id
                };

                _context.Add(newOrder);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        return false;
    }
    public async Task<OrderEditViewModel> GetOrderEditViewModelAsync(int id)
    {
        var clientsList = await _context.Client.ToListAsync();
        var productsList = await _context.Product.ToListAsync();

        var order = await _context.Order
            .Include(x => x.Products)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (order == null)
        {
            return null;
        }

        var orderEdit = new OrderEditViewModel
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            Products = productsList,
            ClientId = order.ClientId,
            Clients = clientsList,
            ProductIds = order.Products?.Select(p => p.Id).ToList() ?? new List<int>()
        };

        return orderEdit;
    }
    public async Task<bool> EditOrderAsync(int id, OrderEditViewModel orderView)
    {
    var validationContext = new ValidationContext(orderView, serviceProvider: null, items: null);
    var validationResults = new List<ValidationResult>();

    if (Validator.TryValidateObject(orderView, validationContext, validationResults, validateAllProperties: true))
    {
        try
        {
            var order = await _context.Order
                .Include(o => o.Products)
                .Include(o => o.Client)
                .Include(o => o.ShippingAddress)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return false;
            }

            order.OrderDate = orderView.OrderDate;
            order.ClientId = orderView.ClientId;
            order.Client = await _context.Client.FirstOrDefaultAsync(x => x.Id == orderView.ClientId);
            order.ShippingAddress = await _context.Address.FirstOrDefaultAsync(x => x.ClientId == orderView.ClientId);
            order.Products = await _context.Product.Where(p => orderView.ProductIds.Contains(p.Id)).ToListAsync();

            _context.Update(order);
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
public async Task<Order> GetOrderAsync(int id)
{
      if (id <= 0)
    {
        return null;
    }

    var order = await _context.Order
        .Include(o => o.Client)
        .Include(o => o.ShippingAddress)
        .FirstOrDefaultAsync(m => m.Id == id);

    if (order == null)
    {
        return null;
    }

    return order;
}

public async Task<bool> DeleteOrderAsync(int id)
{
    if (_context.Order == null)
    {
        return false;
    }

    var order = await _context.Order.FindAsync(id);
    
    if (order != null)
    {
        _context.Order.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }

    return false;
}
}
