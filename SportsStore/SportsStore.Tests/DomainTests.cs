using System.Collections.Generic;
using System.Linq;
using Moq;
using SportsStore.Client.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class DomainTests
    {
        [Fact]
        public void Can_Calculate_Cart_Total()
        {
            // Arrange
            var p1 = new Product { ProductId = 1, Name = "P1", Price = 10.00m };
            var p2 = new Product { ProductId = 2, Name = "P2", Price = 25.50m };
            var cart = new Cart();

            // Act
            cart.AddItem(p1, 2); // 20.00
            cart.AddItem(p2, 1); // 25.50

            // Assert
            Assert.Equal(45.50m, cart.ComputeTotalValue());
        }

        [Fact]
        public void Can_Add_New_Lines()
        {
            // Arrange
            var p1 = new Product { ProductId = 1, Name = "P1", Price = 10.00m };
            var p2 = new Product { ProductId = 2, Name = "P2", Price = 25.50m };
            var cart = new Cart();

            // Act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            var results = cart.Lines.ToArray();

            // Assert
            Assert.Equal(2, results.Length);
            Assert.Equal(p1, results[0].Product);
            Assert.Equal(p2, results[1].Product);
        }

        [Fact]
        public void Can_Increment_Quantity_For_Existing_Lines()
        {
            // Arrange
            var p1 = new Product { ProductId = 1, Name = "P1", Price = 10.00m };
            var cart = new Cart();

            // Act
            cart.AddItem(p1, 1);
            cart.AddItem(p1, 10);
            var results = cart.Lines.ToArray();

            // Assert
            Assert.Single(results);
            Assert.Equal(11, results[0].Quantity);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            // Arrange
            var p1 = new Product { ProductId = 1, Name = "P1", Price = 10.00m };
            var p2 = new Product { ProductId = 2, Name = "P2", Price = 25.50m };
            var cart = new Cart();

            // Act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 3);
            cart.RemoveLine(p1);
            var results = cart.Lines.ToArray();

            // Assert
            Assert.Single(results);
            Assert.Equal(2, results[0].ProductId);
        }
    }
}
