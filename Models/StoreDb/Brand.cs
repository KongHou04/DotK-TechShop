using System.ComponentModel.DataAnnotations;

namespace DotK_TechShop.Models.Db;

public class Brand
{
    [Key]
    public int BrandID { get; set; }


    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;


    // References
    public ICollection<Product>? Products { get; set; }
}
