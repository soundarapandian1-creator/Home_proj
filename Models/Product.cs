using System.ComponentModel.DataAnnotations;

namespace simpleApiDemo.Models
{
    public class Product
    {
        public int Id {get; set;}
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string Name{get; set;} = string.Empty;
        [Required(ErrorMessage = "Product price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Product price must be a positive number.")]
        public decimal Price{get; set;}
    }
}