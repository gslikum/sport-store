using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportsStore.Client.Models;

namespace SportsStore.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<CartLine> CartLines => Set<CartLine>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure 3NF relationship between Category and Product
        builder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ensure category name is unique
        builder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        // Seed Categories
        builder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Watersports", Description = "Things for the water" },
            new Category { CategoryId = 2, Name = "Soccer", Description = "All soccer gear" },
            new Category { CategoryId = 3, Name = "Chess", Description = "Mind games" }
        );

        // Seed Products (Adapted from the book, adding StockQuantity and ImagePath)
        builder.Entity<Product>().HasData(
            new Product { ProductId = 1, Name = "Kayak", Description = "A boat for one person", Price = 275.00m, CategoryId = 1, StockQuantity = 10, ImagePath = "/images/kayak.png" },
            new Product { ProductId = 2, Name = "Lifejacket", Description = "Protective and fashionable", Price = 48.95m, CategoryId = 1, StockQuantity = 25, ImagePath = "/images/lifejacket.png" },
            new Product { ProductId = 3, Name = "Soccer Ball", Description = "FIFA-approved size and weight", Price = 19.50m, CategoryId = 2, StockQuantity = 50, ImagePath = "/images/soccer_ball.png" },
            new Product { ProductId = 4, Name = "Corner Flags", Description = "Give your pitch a professional touch", Price = 34.95m, CategoryId = 2, StockQuantity = 8, ImagePath = "/images/corner_flags.png" },
            new Product { ProductId = 5, Name = "Stadium", Description = "Flat-packed 35,000-seat stadium", Price = 79500.00m, CategoryId = 2, StockQuantity = 1, ImagePath = "/images/stadium.png" },
            new Product { ProductId = 6, Name = "Thinking Cap", Description = "Improve brain efficiency by 75%", Price = 16.00m, CategoryId = 3, StockQuantity = 15, ImagePath = "/images/thinking_cap.png" },
            new Product { ProductId = 7, Name = "Unsteady Chair", Description = "Secretly give your opponent a disadvantage", Price = 29.95m, CategoryId = 3, StockQuantity = 4, ImagePath = "/images/unsteady_chair.png" },
            new Product { ProductId = 8, Name = "Human Chess Board", Description = "A fun game for the family", Price = 75.00m, CategoryId = 3, StockQuantity = 2, ImagePath = "/images/human_chess_board.png" },
            new Product { ProductId = 9, Name = "Bling-Bling King", Description = "Gold-plated, diamond-studded King", Price = 1200.00m, CategoryId = 3, StockQuantity = 1, ImagePath = "/images/bling_bling_king.png" }
        );
    }
}

