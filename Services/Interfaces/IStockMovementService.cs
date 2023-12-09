using Examenes.Models;
using Examenes.ViewModels;

namespace  Examenes.Services;
    public interface IStockMovementService{
        Task CreateStockOutMovementAsync(Dictionary<int, int> ProductStock, int idOrder);
        Task CreateStockInMovementAsync(Dictionary<int, int> productStock, int idOrder);
        Task<Dictionary<int, int>> GetStockMovementsByOrderIdAsync(int idOrder);
        Task<Dictionary<int, int>> GetStockResumeByOrderIdAsync(int idOrder);
    }