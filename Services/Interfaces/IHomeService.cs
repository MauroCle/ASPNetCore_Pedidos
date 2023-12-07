using Examenes.ViewModels;

namespace  Examenes.Services;
public interface IHomeService{
        Task<bool> AnyClientAvailable();
        Task<bool> AnyProductAvailable();
        Task<bool> AnyAddressAvailable();
        Task<bool> AnyOrderAvailable();
}