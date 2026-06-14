using System;
using SportsStore.Client.Models;

namespace SportsStore.Client.Services
{
    // Observer Pattern (CIS 518) to notify components of cart updates
    public class CartStateService
    {
        public Cart Cart { get; } = new();

        public event Action? OnChange;

        public void AddItem(Product product, int quantity)
        {
            Cart.AddItem(product, quantity);
            NotifyStateChanged();
        }

        public void RemoveLine(Product product)
        {
            Cart.RemoveLine(product);
            NotifyStateChanged();
        }

        public void UpdateQuantity(Product product, int quantity)
        {
            var line = Cart.Lines.FirstOrDefault(l => l.ProductId == product.ProductId);
            if (line != null)
            {
                line.Quantity = quantity;
                NotifyStateChanged();
            }
        }

        public void Clear()
        {
            Cart.Clear();
            NotifyStateChanged();
        }

        protected void NotifyStateChanged() => OnChange?.Invoke();
    }
}
