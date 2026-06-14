using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Client.Models
{
    public class Cart
    {
        private readonly List<CartLine> _lines = new();

        public virtual IEnumerable<CartLine> Lines => _lines;

        public virtual void AddItem(Product product, int quantity)
        {
            var line = _lines
                .FirstOrDefault(p => p.ProductId == product.ProductId);

            if (line == null)
            {
                _lines.Add(new CartLine
                {
                    ProductId = product.ProductId,
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product) =>
            _lines.RemoveAll(l => l.ProductId == product.ProductId);

        public virtual decimal ComputeTotalValue() =>
            _lines.Sum(e => e.Product.Price * e.Quantity);

        public virtual void Clear() => _lines.Clear();
    }
}
