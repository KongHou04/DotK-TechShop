using System.ComponentModel.DataAnnotations;

namespace DotK_TechShop.Models.Db;

public class Discount
{
    [Key]
    public int DiscountID { get; set; }


    [Required]
    [Range(0, 100)]
    public int Value { get; set; }


    [Required]
    public DateTime StartDate { get; set; }


    [Required]
    public DateTime EndDate { get; set; }


    // Foreign Key
    public int ProductID { get; set; }


    // References
    public Product? Product { get; set; }
}