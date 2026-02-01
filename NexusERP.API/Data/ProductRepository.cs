using NexusERP.API.Domain;
using NexusERP.API.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NexusERP.API.Data
{
    public class ProductRepository : IProductRepository
    {
        private AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync<Product>();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            if(_context == null)
            {
                throw new InvalidOperationException("Database context is not initialized.");
            }

            if(id == Guid.Empty)
            {
                throw new ArgumentException("Invalid product ID.", nameof(id));
            }

            return await _context.Products.FindAsync(id);
        }

        public async Task AddAsync(Product product)
        {
            if (product == null) throw new ArgumentException("Product cannot be null.", nameof(product));
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            if (product == null) throw new ArgumentException("Product cannot be null.", nameof(product));
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await GetByIdAsync(id);

            if (product == null) throw new ArgumentException("Product not found.", nameof(id));
            
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}