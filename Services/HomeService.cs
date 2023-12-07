using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examenes.Data;
using Examenes.Models;
using Examenes.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Examenes.Services
{
    public class HomeService : IHomeService
    {
        private readonly YaPedidosContext _context;

        public HomeService(YaPedidosContext context)
        {
            _context = context;
        }

        public async Task<bool> AnyClientAvailable()
        {
            return await _context.Client.AnyAsync();
        }

        public async Task<bool> AnyProductAvailable()
        {
            return await _context.Product.AnyAsync();
        }

        public async Task<bool> AnyAddressAvailable()
        {
            return await _context.Address.AnyAsync();
        }

        public async Task<bool> AnyOrderAvailable()
        {
            return await _context.Order.AnyAsync();
        }
    }
}
