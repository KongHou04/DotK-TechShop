using System.ComponentModel.DataAnnotations;

namespace DotK_TechShop.Models.Db;

public class OrderDetail
{
    [Key]
    public int ODID { get; set; }


    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }


    [Required]
    [Range(0, double.MaxValue)]
    public double UnitPrice { get; set; }


    // Forein Key
    public int OrderID { get; set; }
    public int ProductID { get; set; }


    // References
    public Order? Order { get; set; }
    public Product? Product { get; set; }
}
