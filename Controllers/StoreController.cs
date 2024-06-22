using DotK_TechShop.Models;
using DotK_TechShop.Models.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DotK_TechShop.Controllers
{
    public class StoreController : Controller
    {
        private readonly MyDbContext _context;
        private readonly double PRODUCT_PER_PAGE = 16;

        public StoreController(MyDbContext context)
        {
            _context = context;
        }


        public IActionResult Index(StoreFilterViewModel filterModel, int page = 1)
        {
            filterModel = filterModel ?? new StoreFilterViewModel();
            filterModel.SortBy = filterModel.SortBy ?? "Default";
            filterModel.Brand = filterModel.Brand ?? "All";
            double min = (filterModel.FromPrice <= filterModel.ToPrice) ? filterModel.FromPrice : filterModel.ToPrice;
            double max = (filterModel.FromPrice >= filterModel.ToPrice) ? filterModel.FromPrice : filterModel.ToPrice;
            var products = _context.Products.ToList();
            if (filterModel.Brand != null && filterModel.Brand != "All")
                products = products.Where(p =>p.BrandID == int.Parse(filterModel.Brand)).ToList();
            if (filterModel.KeyName != null)
                products = products.Where(p => p.Name.ToLower().Contains(filterModel.KeyName)).ToList();
            products = products.Where(p => p.Price >= min && p.Price <= max).ToList();
            var sortOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Default", Text = "Default" },
                new SelectListItem { Value = "PriceAsc", Text = "Price up" },
                new SelectListItem { Value = "PriceDesc", Text = "Price down" },
            };
            filterModel.SortOptions = sortOptions;
            filterModel.Brands = _context.Brands.Select(b => new SelectListItem { Value = b.BrandID.ToString(), Text = b.Name}).ToList();
            filterModel.Brands.Add(new SelectListItem("All", "All"));
            filterModel.Brands = filterModel.Brands.OrderBy(b => b.Text).ToList();
            switch (filterModel.SortBy?.ToLower())
            {
                default:
                    products = products.OrderBy(p => p.Name).ToList();
                    break;
                case "priceasc":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case "pricedesc":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
            }
            products = products.Skip((page - 1) * (int)PRODUCT_PER_PAGE)
                .Take((int)PRODUCT_PER_PAGE).ToList();
            ViewData["Products"] = products;
            ViewData["Page"] = page;
            return View("Index", filterModel);
        }


        public IActionResult Detail(int id)
        {
            var p = _context.Products.Include(p => p.Discounts).Include(p => p.Brand)
                .FirstOrDefault(p => p.ProductID == id);
            if (p == null)
                return NotFound();
            return View("detail", p);
        }
    }
}
