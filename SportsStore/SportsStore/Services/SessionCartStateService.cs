using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SportsStore.Client.Models;
using SportsStore.Client.Services;

namespace SportsStore.Services
{
    // Inherits CartStateService to add session persistence (CIS 502/555)
    // Refactored to only serialize ProductId/Quantity (DTO) to prevent circular reference cycles
    public class SessionCartStateService : CartStateService
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly IStoreRepository _repository;
        private const string CartSessionKey = "CartSession";

        public SessionCartStateService(ProtectedSessionStorage sessionStorage, IStoreRepository repository)
        {
            _sessionStorage = sessionStorage;
            _repository = repository;
        }

        private Task? _loadTask;

        public Task LoadCartAsync()
        {
            if (_loadTask == null)
            {
                _loadTask = DoLoadCartAsync();
            }
            return _loadTask;
        }

        private async Task DoLoadCartAsync()
        {
            try
            {
                var result = await _sessionStorage.GetAsync<string>(CartSessionKey);
                if (result.Success && !string.IsNullOrEmpty(result.Value))
                {
                    var items = JsonSerializer.Deserialize<List<SessionCartLine>>(result.Value);
                    Cart.Clear();
                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            var product = _repository.Products.FirstOrDefault(p => p.ProductId == item.ProductId);
                            if (product != null)
                            {
                                Cart.AddItem(product, item.Quantity);
                            }
                        }
                    }
                    NotifyStateChanged();
                }
            }
            catch (Exception)
            {
                // JS interop is disabled during pre-rendering, safe to ignore
                _loadTask = null; // Clear if it failed due to pre-rendering
            }
        }

        public async Task SaveCartAsync()
        {
            try
            {
                var items = Cart.Lines.Select(line => new SessionCartLine
                {
                    ProductId = line.ProductId,
                    Quantity = line.Quantity
                }).ToList();

                var json = JsonSerializer.Serialize(items);
                await _sessionStorage.SetAsync(CartSessionKey, json);
            }
            catch (Exception)
            {
                // JS interop disabled, safe to ignore
            }
        }
    }

    public class SessionCartLine
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
