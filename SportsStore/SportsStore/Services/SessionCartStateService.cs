using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SportsStore.Client.Models;
using SportsStore.Client.Services;

namespace SportsStore.Services
{
    // Inherits CartStateService to add session persistence (CIS 502/555)
    public class SessionCartStateService : CartStateService
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private const string CartSessionKey = "CartSession";

        public SessionCartStateService(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
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
                    var lines = JsonSerializer.Deserialize<List<CartLine>>(result.Value);
                    Cart.Clear();
                    if (lines != null)
                    {
                        foreach (var line in lines)
                        {
                            Cart.AddItem(line.Product, line.Quantity);
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
                var json = JsonSerializer.Serialize(Cart.Lines, new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles // Avoid loop circular references
                });
                await _sessionStorage.SetAsync(CartSessionKey, json);
            }
            catch (Exception)
            {
                // JS interop disabled, safe to ignore
            }
        }
    }
}
