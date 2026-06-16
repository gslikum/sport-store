using Bunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SportsStore.Data;
using SportsStore.Components;
using SportsStore.Components.Pages;
using SportsStore.Client.Models;
using SportsStore.Client.Services;
using Xunit;

namespace SportsStore.Tests
{
    public class ComponentTests : BunitContext
    {
        [Fact]
        public void Pager_Renders_Correct_Page_Links()
        {
            // Arrange
            var cut = Render<Pager>(parameters => parameters
                .Add(p => p.CurrentPage, 2)
                .Add(p => p.TotalPages, 3)
                .Add(p => p.PageUrl, page => $"/page-{page}")
            );

            // Act
            var links = cut.FindAll("a");

            // Assert
            Assert.Equal(5, links.Count); // Previous, 1, 2, 3, Next
            Assert.Equal("/page-1", links[0].GetAttribute("href")); // Previous points to 1
            Assert.Equal("/page-2", cut.Find(".active a").GetAttribute("href")); // Active page is 2
            Assert.Equal("/page-3", links[4].GetAttribute("href")); // Next points to 3
        }

        [Fact]
        public void Home_Renders_Watersports_Category()
        {
            // Arrange
            var mockRepo = new Mock<IStoreRepository>();
            var products = new List<Product>
            {
                new Product { ProductId = 1, Name = "P1", Category = new Category { Name = "Watersports" } },
                new Product { ProductId = 2, Name = "P2", Category = new Category { Name = "Watersports" } }
            }.AsQueryable();
            mockRepo.Setup(r => r.Products).Returns(products);
            mockRepo.Setup(r => r.Categories).Returns(new List<Category>
            {
                new Category { CategoryId = 1, Name = "Watersports" }
            }.AsQueryable());

            Services.AddSingleton<IStoreRepository>(mockRepo.Object);
            Services.AddSingleton<CartStateService>(new CartStateService());

            // Act
            var cut = Render<Home>(parameters => parameters
                .Add(p => p.Category, "Watersports")
            );

            // Assert
            Assert.Contains("P1", cut.Markup);
            Assert.Contains("P2", cut.Markup);
        }

        [Fact]
        public void Home_Renders_Watersports_Category_Lowercase()
        {
            // Arrange
            var mockRepo = new Mock<IStoreRepository>();
            var products = new List<Product>
            {
                new Product { ProductId = 1, Name = "P1", Category = new Category { Name = "Watersports" } },
                new Product { ProductId = 2, Name = "P2", Category = new Category { Name = "Watersports" } }
            }.AsQueryable();
            mockRepo.Setup(r => r.Products).Returns(products);
            mockRepo.Setup(r => r.Categories).Returns(new List<Category>
            {
                new Category { CategoryId = 1, Name = "Watersports" }
            }.AsQueryable());

            Services.AddSingleton<IStoreRepository>(mockRepo.Object);
            Services.AddSingleton<CartStateService>(new CartStateService());

            // Act
            var cut = Render<Home>(parameters => parameters
                .Add(p => p.Category, "watersports")
            );

            // Assert
            Assert.Contains("P1", cut.Markup);
            Assert.Contains("P2", cut.Markup);
        }
        [Fact]
        public void Home_Integration_Category_Tests()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Data Source=/Users/gerrell/Documents/Sports Store/SportsStore/SportsStore/Data/app.db")
                .Options;

            using var context = new ApplicationDbContext(options);
            var efRepo = new EFStoreRepository(context);
            var cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions());
            var cachedRepo = new CachedStoreRepository(efRepo, cache);

            Services.AddSingleton<IStoreRepository>(cachedRepo);
            Services.AddSingleton<CartStateService>(new CartStateService());

            // Act & Assert
            // Test Chess
            var cutChess = Render<Home>(parameters => parameters.Add(p => p.Category, "Chess"));
            Assert.Contains("Thinking Cap", cutChess.Markup);

            // Test Watersports
            var cutWater = Render<Home>(parameters => parameters.Add(p => p.Category, "Watersports"));
            Assert.Contains("Kayak", cutWater.Markup);

            // Test Soccer
            var cutSoccer = Render<Home>(parameters => parameters.Add(p => p.Category, "Soccer"));
            Assert.Contains("Soccer Ball", cutSoccer.Markup);
        }

        [Fact]
        public void Home_Add_To_Cart_Click_Updates_CartState()
        {
            // Arrange
            var mockRepo = new Mock<IStoreRepository>();
            var product = new Product 
            { 
                ProductId = 1, 
                Name = "P1", 
                Price = 100, 
                Category = new Category { Name = "Watersports" },
                StockQuantity = 5 
            };
            var products = new List<Product> { product }.AsQueryable();
            mockRepo.Setup(r => r.Products).Returns(products);
            mockRepo.Setup(r => r.Categories).Returns(new List<Category>
            {
                new Category { CategoryId = 1, Name = "Watersports" }
            }.AsQueryable());

            var cartState = new CartStateService();
            Services.AddSingleton<IStoreRepository>(mockRepo.Object);
            Services.AddSingleton<CartStateService>(cartState);

            // Act
            var cut = Render<Home>();
            
            // Find the "Add To Cart" button
            var button = cut.Find("button.btn-primary-custom");
            button.Click();

            // Assert
            Assert.Single(cartState.Cart.Lines);
            Assert.Equal(1, cartState.Cart.Lines.First().Product.ProductId);
            Assert.Equal(1, cartState.Cart.Lines.First().Quantity);
        }
    }
}

