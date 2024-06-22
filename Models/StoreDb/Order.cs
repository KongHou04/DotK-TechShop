using System.ComponentModel.DataAnnotations;

namespace DotK_TechShop.Models.Db;

public class Order
{
    [Key]
    public int OrderID { get; set; }


    [Required]
    public DateTime OrderDate { get; set; }


    [Required]
    [Range(0, double.MaxValue)]
    public double OrderTotal { get; set; }


    [Range(0, double.MaxValue)]
    public double Discount { get; set; } = 0;


    [Required]
    [Range(0, double.MaxValue)]
    public double FinalTotal { get; set; }


    // Forein Key
    [Required]
    public string Phone { get; set; } = string.Empty;


    // References
    public Customer? Customer { get; set; }
    public ICollection<OrderDetail>? OrderDetails { get; set; }
}
