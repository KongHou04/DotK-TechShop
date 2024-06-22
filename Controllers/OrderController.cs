using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotK_TechShop.Models.Db;
using Microsoft.AspNetCore.Authorization;
using DotK_TechShop.Models;
using DotK_TechShop.Services;

namespace DotK_TechShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly MyDbContext _context;

        public OrderController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.Orders.Include(o => o.Customer).OrderByDescending(o => o.OrderDate);
            return View(await myDbContext.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Order/Create
        //public IActionResult Create()
        //{
        //    ViewData["Phone"] = new SelectList(_context.Customers, "Phone", "Phone");
        //    return View();
        //}

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,OrderDate,OrderTotal,Discount,FinalTotal,Phone")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Phone"] = new SelectList(_context.Customers, "Phone", "Phone", order.Phone);
            return View(order);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["Phone"] = new SelectList(_context.Customers, "Phone", "Phone", order.Phone);
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,OrderDate,OrderTotal,Discount,FinalTotal,Phone")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Phone"] = new SelectList(_context.Customers, "Phone", "Phone", order.Phone);
            return View(order);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }


        public IActionResult Create()
        {
            var cartData = HttpContext.Session.GetObject<List<CartItemViewModel>>("EmployeeCart");
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
                if (HttpContext.Session.GetObject<List<CartItemViewModel>>("EmployeeCart") == null)
                {
                    var cart = new List<CartItemViewModel>();
                    cart.Add(new CartItemViewModel(product));
                    HttpContext.Session.SetObject("EmployeeCart", cart);
                }
                else
                {
                    var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>("EmployeeCart");
                    if (cart != null)
                    {
                        cart.Add(new CartItemViewModel(product));
                        HttpContext.Session.SetObject("EmployeeCart", cart);
                    }
                }
            }

            return RedirectToAction("Create");
        }

        [HttpPost]
        public IActionResult IncreaseQuantity(int productID)
        {
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>("EmployeeCart");

            if (cart != null)
            {
                var item = cart.FirstOrDefault(i => i.Product.ProductID == productID);
                if (item != null)
                {
                    item.Quantity++;
                    if (item.Total > 150000000)
                        item.Quantity--;
                    HttpContext.Session.SetObject("EmployeeCart", cart);
                }
            }

            return RedirectToAction("Create");
        }

        [HttpPost]
        public IActionResult DecreaseQuantity(int productID)
        {
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>("EmployeeCart");

            if (cart != null)
            {
                var item = cart.FirstOrDefault(i => i.Product.ProductID == productID);
                if (item != null && item.Quantity != 1)
                {
                    item.Quantity--;
                    HttpContext.Session.SetObject("EmployeeCart", cart);
                }
            }

            return RedirectToAction("Create");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productID)
        {
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>("EmployeeCart");

            if (cart != null)
            {
                var item = cart.FirstOrDefault(i => i.Product.ProductID == productID);
                if (item != null)
                {
                    cart.Remove(item);
                }
                HttpContext.Session.SetObject<List<CartItemViewModel>>("EmployeeCart", cart);
            }
            return RedirectToAction("Create");
        }

        [HttpPost]
        public IActionResult Order(Customer customer, Order order)
        {
            var cartData = HttpContext.Session.GetObject<List<CartItemViewModel>>("EmployeeCart");

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
            HttpContext.Session.Remove("EmployeeCart");


            return RedirectToAction("Details", new { id = order.OrderID });
        }
    }
}
