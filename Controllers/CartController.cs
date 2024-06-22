using DotK_TechShop.Models;
using DotK_TechShop.Models.Db;
using DotK_TechShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace DotK_TechShop.Controllers
{
    public class CartController : Controller
    {
        private readonly MyDbContext _context;
        public CartController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cartData = HttpContext.Session.GetObject<List<CartItemViewModel>>("Cart");
            if (cartData != null)
                return View(cartData);
            //else
            //{
            //    return View(new List<Product>());
            //}
            return View();
        }

        [HttpPost]
        public IActionResult AddToCart(int productID)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductID == productID);

            if (product != null)
            {
                if (HttpContext.Session.GetObject<List<CartItemViewModel>>("Cart") == null)
                {
                    var cart = new List<CartItemViewModel>();
                    cart.Add(new CartItemViewModel(product));
                    HttpContext.Session.SetObject("Cart", cart);
                }
                else
                {
                    var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>("Cart");
                    if (cart != null)
                    {
                        cart.Add(new CartItemViewModel(product));
                        HttpContext.Session.SetObject("Cart", cart);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult IncreaseQuantity(int productID)
        {
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>("Cart");

            if (cart != null)
            {
                var item = cart.FirstOrDefault(i => i.Product.ProductID == productID);
                if (item != null)
                {
                    item.Quantity++;
                    if (item.Total > 150000000)
                        item.Quantity--;
                    HttpContext.Session.SetObject("Cart", cart);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DecreaseQuantity(int productID)
        {
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>("Cart");

            if (cart != null)
            {
                var item = cart.FirstOrDefault(i => i.Product.ProductID == productID);
                if (item != null && item.Quantity != 1)
                {
                    item.Quantity--;
                    HttpContext.Session.SetObject("Cart", cart);
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int productID)
        {
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>("Cart");

            if (cart != null)
            {
                var item = cart.FirstOrDefault(i => i.Product.ProductID == productID);
                if (item != null)
                {
                    cart.Remove(item);
                }
                HttpContext.Session.SetObject<List<CartItemViewModel>>("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Order(Customer customer, Order order)
        {
            var cartData = HttpContext.Session.GetObject<List<CartItemViewModel>>("Cart");

            // Checks if customer infor is invalid
            var isValidCustomer = true;
            if (customer?.Phone == null)
                isValidCustomer = false;
            if (customer?.FullName == null)
                isValidCustomer = false;
            if (customer?.Email == null)
                isValidCustomer = false;
            if (customer?.Address == null)
                isValidCustomer = false;
            if (!isValidCustomer)
            {
                ViewData["CustomerInvalidMessage"] = "Please fill in the form correctly";
                if (cartData != null)
                    return View("Index", cartData);
                return View("Index");
            }
            // returns to View
            if (customer == null)
            {
                if (cartData != null)
                    return View("Index", cartData);
                return View("Index");
            }


            var existingCustomer = _context.Customers.FirstOrDefault(c => c.Phone == customer.Phone);
            if (existingCustomer != null)
            {
                existingCustomer.FullName = customer.FullName;
                existingCustomer.Email = customer.Email;
                existingCustomer.Address = customer.Address;
                _context.SaveChanges();
            }
            else
            {
                _context.Customers.Add(customer);
            }

            order.Customer = existingCustomer ?? customer;
            order.OrderDate = DateTime.Now;
            _context.Orders.Add(order);
            _context.SaveChanges();

            if (cartData != null)
                foreach (var item in cartData)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderID = order.OrderID,
                        ProductID = item.Product.ProductID, 
                        Quantity = item.Quantity,
                        UnitPrice = item.Product.Price
                    };
                    _context.OrderDetails.Add(orderDetail);
                }
            _context.SaveChanges();
            HttpContext.Session.Remove("Cart");


            return View("Success", order.OrderID);
        }

        public IActionResult CheckOrder(int orderID)
        {
            var myDbContext = _context.Orders.Include(o => o.Customer).OrderByDescending(o => o.OrderDate).ToList();
            var orders = _context.Orders.ToList();
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Include(o => o.Customer)
                .FirstOrDefault(o => o.OrderID == orderID);
            return View("Check", order);
        }

    }
}
