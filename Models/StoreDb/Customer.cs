using System.ComponentModel.DataAnnotations;

namespace DotK_TechShop.Models.Db;

public class Customer
{
    [Key]
    public string Phone { get; set; } = string.Empty;


    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;


    [StringLength(100)]
    public string Email { get; set; } = string.Empty;


    [Required]
    [StringLength(455)]
    public string Address { get; set; } = string.Empty;


    // References
    public ICollection<Order>? Orders { get; set; }
}
