using System.ComponentModel.DataAnnotations;

namespace SportsStore.Client.Models
{
    public class Category
    {
        public long CategoryId { get; set; }

        [Required(ErrorMessage = "Please enter a category name")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a category description")]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        // Navigation property for 1-to-many relationship
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
