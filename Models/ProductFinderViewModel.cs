using DotK_TechShop.Models.Db;

namespace DotK_TechShop.Models;


public class ProductFinderViewModel
{
    public List<Product> Products = new List<Product>();
    public string NameKey { get; set;} = "";
}