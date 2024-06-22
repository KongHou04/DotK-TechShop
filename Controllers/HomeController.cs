using DotK_TechShop.Models;
using DotK_TechShop.Models.Db;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DotK_TechShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext _context;

        public HomeController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hotProducts = _context.Products.Include(p => p.Discounts).ToList();
            ViewData["HotProducts"] = hotProducts;
            return View();
        }
        public IActionResult Cart()
        {
            return View();
        }
    }
}
