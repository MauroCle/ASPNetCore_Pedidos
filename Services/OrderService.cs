using System.ComponentModel.DataAnnotations;
using Examenes.Data;
using Examenes.Models;
using Examenes.ViewModels;
using Microsoft.EntityFrameworkCore;
using Examenes.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

public class OrderService : IOrderService
{
    private readonly YaPedidosContext _context;
    private readonly IProductService _productService;
    private readonly IClientService _clientService;
    private readonly IStockMovementService _stockMovementService;
    public OrderService(YaPedidosContext context, IProductService productService, IClientService clientService, IStockMovementService stockMovementService)
    {
        _context = context;
        _productService = productService;
        _clientService = clientService;
        _stockMovementService = stockMovementService;
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

    // public async Task<List<OrderViewModel>> GetActiveOrdersAsync(string nameFilter)
    // {
    //     var query = _context.Order.Where(x=> x.Active==true).AsQueryable();

    //     if (!string.IsNullOrEmpty(nameFilter))
    //     {
    //         query = query.Where(x =>
    //             x.Client.FirstName.ToLower().Contains(nameFilter.ToLower()) ||
    //             x.Client.LastName.ToLower().Contains(nameFilter.ToLower()) ||
    //             x.Client.Email.ToLower().Contains(nameFilter.ToLower()) ||
    //             x.ShippingAddress.City.ToLower().Contains(nameFilter.ToLower()) ||
    //             x.ShippingAddress.Street.ToLower().Contains(nameFilter.ToLower()) ||
    //             x.ShippingAddress.Number.ToString().Contains(nameFilter.ToLower())
    //         );
    //     }

    //     var orders = await query
    //         .Select(item => new OrderViewModel
    //         {
    //             Id = item.Id,
    //             OrderDate = item.OrderDate,
    //             ShippingAddressData = $"{item.ShippingAddress.City} - {item.ShippingAddress.Street} {item.ShippingAddress.Number} ({item.ShippingAddress.PostalCode})",
    //             ProductsQuantity = item.Products.Count(),
    //             ClientData = $"{item.Client.FirstName} {item.Client.LastName} - {item.Client.Email}"
    //         })
    //         .ToListAsync();

    //     return orders;
    // }
    

    public async Task<OrderDetailViewModel> GetOrderDetailsAsync(int id)
    {
        var order = await _context.Order
            .Include(o => o.Client)
            .Include(o => o.ShippingAddress)
            .Include(o => o.Products)
            .Include(o => o.StockMovements)
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
            ClientData = $"{order.Client.FirstName} {order.Client.LastName} - ({order.Client.PhoneNumber}) || ({order.Client.Email})",
            ProductStockDictionary = await _stockMovementService.GetStockResumeByOrderIdAsync(id)

        };

        return orderDetail;
    }
    public async Task<List<Order>> GetOrdersWithProductsAsync()
    {

        List<Order> orders = await _context.Order.Include(o => o.Products).ToListAsync();

        if (orders == null)
            return orders = new List<Order>();
        else
            return orders;
    }
    public async Task<bool> CreateOrderAsync(OrderCreateViewModel viewModel)
    {
        Client client = await _clientService.GetClientByIdWithAddressAsync(viewModel.ClientId);
        var productsList = await _productService.GetProductsModelAsync();  //_context.Product.ToListAsync();
        var products = new List<Product>();



        if (client != null)
        {
            foreach (var item in viewModel.ProductStockDictionary)
            {
                var product = productsList.FirstOrDefault(x => x.Id == item.Key);
                if (product != null && viewModel.ProductStockDictionary[item.Key] > 0)
                {
                    products.Add(product);
                }
            }

            Order newOrder = new Order
            {
                OrderDate = viewModel.OrderDate,
                AddressId = client.Address.Id,
                ShippingAddress = client.Address,
                Products = products,
                ClientId = client.Id,
                //ProductStockDictionary = viewModel.ProductStockDictionary

            };

            _context.Add(newOrder);
            await _context.SaveChangesAsync();

            await _stockMovementService.CreateStockOutMovementAsync(viewModel.ProductStockDictionary,newOrder.Id); //TODO probar sin await y ver que pasa

            return true;
        }


        return false;
    }
    public async Task<OrderEditViewModel> GetOrderEditViewModelAsync(int id)
    {
        Order order = await GetOrderAsync(id);
        if (order == null)
        {
            return null;
        }

        List<Client> clientsList = await _clientService.GetAllClientsAsync(""); //await _context.Client.ToListAsync();
        if (clientsList == null)
        {
            return null;
        }

        List<Product> productsList = await _productService.GetProductsModelAsync(); //await _context.Product.ToListAsync();
        if (productsList == null)
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
            ProductIds = order.Products?.Select(p => p.Id).ToList() ?? new List<int>(),
            ProductStockDictionary = await _stockMovementService.GetStockResumeByOrderIdAsync(id)
        };

        return orderEdit;
    }
    public async Task<bool> EditOrderAsync(int id, OrderEditViewModel orderView)
    {
        // var validationContext = new ValidationContext(orderView, serviceProvider: null, items: null);
        // var validationResults = new List<ValidationResult>();

        // if (Validator.TryValidateObject(orderView, validationContext, validationResults, validateAllProperties: true))
        // {
            try
            {
                var order = await _context.Order
                    .Include(o => o.Products)
                    .Include(o => o.Client)
                    .Include(o => o.ShippingAddress)
                    .Include(o => o.StockMovements)
                    .FirstOrDefaultAsync(m => m.Id == id);

                Dictionary<int,int> oldStockMovements = await _stockMovementService.GetStockResumeByOrderIdAsync(order.Id);

                if (order == null)
                {
                    return false;
                }

                foreach (var item in orderView.ProductStockDictionary)
                {
                    if(oldStockMovements.ContainsKey(item.Key))
                    {
                        if (item.Value > oldStockMovements[item.Key]) //Hay que chequear si el oldStockMovements tiene la key de [item.Key]. Parecido a lo del front
                        {
                            int finalQuantity = item.Value - oldStockMovements[item.Key];

                            Dictionary<int, int> newProductStock = new Dictionary<int, int>
                                {
                                    { item.Key, finalQuantity }
                                };

                            await _stockMovementService.CreateStockOutMovementAsync(newProductStock, order.Id);
                            
                            if(!order.Products.Contains(await _productService.GetProductModelAsync(item.Key)))
                            {
                                
                                order.Products.Add(await _productService.GetProductModelAsync(item.Key));
                            }

                        } 
                        else if(item.Value < oldStockMovements[item.Key])
                        {
                            int finalQuantity = oldStockMovements[item.Key] - item.Value;

                            Dictionary<int, int> newProductStock = new Dictionary<int, int>
                                {
                                    { item.Key, finalQuantity }
                                };

                            await _stockMovementService.CreateStockInMovementAsync(newProductStock, order.Id);
                            
                            if(item.Value==0)
                            {
                                order.Products.Remove(await _productService.GetProductModelAsync(item.Key));
                            }                            

                        } 
                    }else if(item.Value>0)
                    {
                        Dictionary<int, int> newProductStock = new Dictionary<int, int>
                        {
                            { item.Key, item.Value }
                        };

                        await _stockMovementService.CreateStockOutMovementAsync(newProductStock, order.Id);

                        if(orderView.Products != null && !orderView.Products.Contains(await _productService.GetProductModelAsync(item.Key)))
                        {
                            order.Products.Add(await _productService.GetProductModelAsync(item.Key));
                        }
                    }
                    //else si es igual no cambia nada.
                }

                //if (order.ProductStockDictionary != orderView.ProductStockDictionary)
                // {
                //     foreach (var item in orderView.ProductStockDictionary)
                //     {
                //         _productService.DecreaseStockProduct(item.Key, item.Value);
                //     }
                // } Esto ya no iria, lo hace el service de stock

                order.OrderDate = orderView.OrderDate;
                order.ClientId = orderView.ClientId;
                order.Client = await _context.Client.FirstOrDefaultAsync(x => x.Id == orderView.ClientId);
                order.ShippingAddress = await _context.Address.FirstOrDefaultAsync(x => x.ClientId == orderView.ClientId);
                
                //order.ProductStockDictionary = orderView.ProductStockDictionary;


                _context.Update(order);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        // }

        // return false;
    }
    public async Task<bool> EditOrderWithoutProductsAsync(int id, OrderEditViewModel orderView)  //Deprecado
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
            .Include(o => o.Products)
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

            Dictionary<int,int> orderProductsStock = await _stockMovementService.GetStockResumeByOrderIdAsync(order.Id);
            await _stockMovementService.CreateStockInMovementAsync(orderProductsStock, order.Id);

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}
