using Examenes.Data;
using Examenes.Models;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Examenes.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace Examenes.Services
{
    public class StockMovementService : IStockMovementService{
        private readonly YaPedidosContext _context;

        private readonly IProductService _productService;

        public StockMovementService(YaPedidosContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }


        public async Task CreateStockOutMovementAsync(Dictionary<int, int> productStock, int idOrder){

            foreach(var item in productStock){
                if(item.Value >0) //TODO esto podria cambiar si extiendo la funcionalidad de editar orden
                {
                    StockMovement stockMovement = new StockMovement{
                        IdOrder=idOrder,
                        IdProduct=item.Key,
                        Quantity=item.Value,
                        MovementType=MovementType.Out
                    };
                    _context.Add(stockMovement);
                    await _context.SaveChangesAsync();

                    await _productService.DecreaseStockProduct(item.Key, item.Value);
                }
            }
            
        }

        public async Task CreateStockInMovementAsync(Dictionary<int, int> productStock, int idOrder){

            foreach(var item in productStock){
                if(item.Value >0) //TODO esto podria cambiar si extiendo la funcionalidad de editar orden
                {
                    StockMovement stockMovement = new StockMovement{
                        IdOrder=idOrder,
                        IdProduct=item.Key,
                        Quantity=item.Value,
                        MovementType=MovementType.In
                    };
                    _context.Add(stockMovement);
                    await _context.SaveChangesAsync();
                    
                    await _productService.IncreaseStockProduct(item.Key, item.Value);
                }
            }
            
        }

        public async Task<Dictionary<int, int>> GetStockMovementsByOrderIdAsync( int idOrder){

            var movements = await _context.StockMovement.Where(x => x.IdOrder == idOrder).ToListAsync();
            Dictionary<int,int> movementsDictionary = new Dictionary<int,int>();

            if(movements != null)
            {
                foreach(var item in movements){
                    
                    movementsDictionary.Add(item.IdProduct,item.Quantity);
                }
            }

            return movementsDictionary;

        }

            public async Task<Dictionary<int, int>> GetStockResumeByOrderIdAsync(int idOrder){

            var movements = await _context.StockMovement.Where(x => x.IdOrder == idOrder).ToListAsync();
            Dictionary<int,int> movementsDictionary = new Dictionary<int,int>();
            if(movements != null)
            {
                foreach(var item in movements){
                    
                    if(movementsDictionary.ContainsKey(item.IdProduct)){
                        
                        if(item.MovementType==MovementType.Out)
                        movementsDictionary[item.IdProduct] += item.Quantity;
                        if(item.MovementType==MovementType.In)
                        movementsDictionary[item.IdProduct] -= item.Quantity;
                    }
                    else
                    {
                        movementsDictionary.Add(item.IdProduct,item.Quantity);
                    }
                }
            }

            return movementsDictionary;

        }

    }
}