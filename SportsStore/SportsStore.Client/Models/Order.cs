using System.ComponentModel.DataAnnotations;

namespace SportsStore.Client.Models
{
    public class Order
    {
        public long OrderId { get; set; }

        public ICollection<CartLine> Lines { get; set; } = new List<CartLine>();

        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the first address line")]
        public string Line1 { get; set; } = string.Empty;
        public string? Line2 { get; set; }
        public string? Line3 { get; set; }

        [Required(ErrorMessage = "Please enter a city name")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a state name")]
        public string State { get; set; } = string.Empty;

        public string? Zip { get; set; }

        [Required(ErrorMessage = "Please enter a country name")]
        public string Country { get; set; } = string.Empty;

        public bool GiftWrap { get; set; }

        public bool Shipped { get; set; }
    }

    public class CartLine
    {
        public long CartLineId { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
        
        // FK for Order
        public long OrderId { get; set; }
    }
}
