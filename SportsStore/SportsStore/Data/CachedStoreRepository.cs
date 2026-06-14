using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using SportsStore.Client.Models;

namespace SportsStore.Data
{
    // Decorator Pattern (CIS 518) to add caching (CIS 555 Performance Engineering)
    public class CachedStoreRepository : IStoreRepository
    {
        private readonly IStoreRepository _innerRepository;
        private readonly IMemoryCache _cache;
        private const string ProductsCacheKey = "ProductsCacheKey";
        private const string CategoriesCacheKey = "CategoriesCacheKey";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        public CachedStoreRepository(IStoreRepository innerRepository, IMemoryCache cache)
        {
            _innerRepository = innerRepository;
            _cache = cache;
        }

        public IQueryable<Product> Products
        {
            get
            {
                // Cache the evaluated list to avoid repeated SQLite hits
                var cached = _cache.GetOrCreate(ProductsCacheKey, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                    return _innerRepository.Products.ToList();
                });
                return cached!.AsQueryable();
            }
        }

        public IQueryable<Category> Categories
        {
            get
            {
                var cached = _cache.GetOrCreate(CategoriesCacheKey, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                    return _innerRepository.Categories.ToList();
                });
                return cached!.AsQueryable();
            }
        }

        // Orders are volatile, bypass cache for consistency
        public IQueryable<Order> Orders => _innerRepository.Orders;

        public void SaveProduct(Product p)
        {
            _innerRepository.SaveProduct(p);
            ClearCache();
        }

        public void CreateProduct(Product p)
        {
            _innerRepository.CreateProduct(p);
            ClearCache();
        }

        public void DeleteProduct(Product p)
        {
            _innerRepository.DeleteProduct(p);
            ClearCache();
        }

        public void SaveOrder(Order order)
        {
            _innerRepository.SaveOrder(order);
            // Clear product cache because stock levels might have changed
            ClearCache();
        }

        private void ClearCache()
        {
            _cache.Remove(ProductsCacheKey);
            _cache.Remove(CategoriesCacheKey);
        }
    }
}
