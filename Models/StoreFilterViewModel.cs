using DotK_TechShop.Models.Db;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DotK_TechShop.Models;

public class StoreFilterViewModel
{
    public string KeyName { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public double FromPrice { get; set; } = 0;
    public double ToPrice { get; set; } = 50000000;
    public string? SortBy { get; set; }

    public List<SelectListItem> Brands { get; set; } = [];
    public List<SelectListItem> SortOptions { get; set; } = [];
}