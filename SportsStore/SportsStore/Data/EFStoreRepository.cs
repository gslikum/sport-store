using System.Linq;
using Microsoft.EntityFrameworkCore;
using SportsStore.Client.Models;

namespace SportsStore.Data
{
    public class EFStoreRepository : IStoreRepository
    {
        private readonly ApplicationDbContext _context;

        public EFStoreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Apply eager loading (.Include) to avoid N+1 queries (CIS 555 Performance Engineering)
        public IQueryable<Product> Products => _context.Products.Include(p => p.Category);

        public IQueryable<Category> Categories => _context.Categories;

        public IQueryable<Order> Orders => _context.Orders
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product);

        public void SaveProduct(Product p)
        {
            var dbProduct = _context.Products.FirstOrDefault(prod => prod.ProductId == p.ProductId);
            if (dbProduct != null)
            {
                dbProduct.Name = p.Name;
                dbProduct.Description = p.Description;
                dbProduct.Price = p.Price;
                dbProduct.CategoryId = p.CategoryId;
                dbProduct.StockQuantity = p.StockQuantity;
            }
            _context.SaveChanges();
        }

        public void CreateProduct(Product p)
        {
            _context.Products.Add(p);
            _context.SaveChanges();
        }

        public void DeleteProduct(Product p)
        {
            _context.Products.Remove(p);
            _context.SaveChanges();
        }

        public void SaveOrder(Order order)
        {
            // Execute database transaction block to ensure ACID (CIS 515)
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                foreach (var line in order.Lines)
                {
                    // Fetch fresh product data from DB for stock verification
                    var dbProduct = _context.Products.FirstOrDefault(p => p.ProductId == line.ProductId);
                    if (dbProduct == null)
                    {
                        throw new InvalidOperationException($"Product ID {line.ProductId} not found.");
                    }

                    if (dbProduct.StockQuantity < line.Quantity)
                    {
                        throw new InvalidOperationException($"Insufficient stock for '{dbProduct.Name}'. Available: {dbProduct.StockQuantity}, Requested: {line.Quantity}");
                    }

                    // Deduct stock (CIS 555 Performance & Requirements Engineering)
                    dbProduct.StockQuantity -= line.Quantity;
                    
                    // Attach product navigation to CartLine
                    line.Product = dbProduct;
                    _context.Attach(line.Product);
                }

                if (order.OrderId == 0)
                {
                    _context.Orders.Add(order);
                }

                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw; // Re-throw to inform UI
            }
        }
    }
}
