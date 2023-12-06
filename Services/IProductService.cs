using Examenes.ViewModels;

    public interface IProductService
    {
        Task<List<ProductViewModel>> GetProductsAsync(string nameFilter);
        Task<ProductViewModel> GetProductAsync(int id);
        Task CreateProductAsync(ProductViewModel productViewModel);
        Task UpdateProductAsync(ProductViewModel productViewModel);
        Task DeleteProductAsync(int id);
    }
