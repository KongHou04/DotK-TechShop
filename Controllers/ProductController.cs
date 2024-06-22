using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotK_TechShop.Models.Db;
using DotK_TechShop.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using DotK_TechShop.Models;

namespace DotK_TechShop.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly MyDbContext _context;
        private readonly FileUploader _fileUploadSvc;
        private readonly ImageUploader _imgFileCombineSvc;

        public ProductController(MyDbContext context, FileUploader fSvc, ImageUploader icSvc)
        {
            _context = context;
            _fileUploadSvc = fSvc;
            _imgFileCombineSvc = icSvc;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.Products.Include(p => p.Brand);
            return View(await myDbContext.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            if (product.Image != null)
                product.Image = _imgFileCombineSvc.GetImagePath(product.Image);

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["BrandID"] = 
                new SelectList(_context.Brands, "BrandID", "Name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,Name,Price,Image,RAM,Storage,BrandID")] Product product, [Required]IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                product.Image = _fileUploadSvc.Upload(imageFile);
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandID"] = new SelectList(_context.Brands, "BrandID", "Name", product.BrandID);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandID"] = new SelectList(_context.Brands, "BrandID", "Name", product.BrandID);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,Name,Price,Image,RAM,Storage,BrandID")] Product product, IFormFile? imageFile)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    var oldImgFile = _imgFileCombineSvc.GetNormalImageName(product.Image);
                    if (oldImgFile != null)
                        _fileUploadSvc.Delete(oldImgFile);
                    product.Image = _fileUploadSvc.Upload(imageFile);
                }
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
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
            ViewData["BrandID"] = new SelectList(_context.Brands, "BrandID", "Name", product.BrandID);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }


        public IActionResult GetByNamePartialView()
        {
            List<Product> products = new List<Product>();
            string nameKey = "";
            ProductFinderViewModel p = new ProductFinderViewModel();
            p.NameKey = nameKey;
            p.Products = products;
            return PartialView("_ProductFinder", p);
        }

        [HttpGet]
        public IActionResult GetByName(string nameKey = "")
        {
            List<Product> products = _context.Products.Where(p => p.Name.ToLower().Contains(nameKey)).ToList();
            return View("_ProductFinder", products);
        }
    }
}
