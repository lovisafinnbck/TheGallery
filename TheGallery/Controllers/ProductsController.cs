using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheGallery.Models;

namespace TheGallery.Controllers
{
    public class ProductsController : Controller
    {
        private readonly TheGalleryContext _context;

        public ProductsController(TheGalleryContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["ArtistSortParm"] = String.IsNullOrEmpty(sortOrder) ? "artist_desc" : "";
            ViewData["QuantitySortParm"] = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";
            ViewData["ClickSortParm"] = sortOrder == "Click" ? "click_desc" : "Click";
            var products = from p in _context.Product
                           select p;
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name).Include(a => a.Artist).Include(c => c.Category);
                    break;
                case "Price":
                    products = products.OrderBy(p => p.Price).Include(a => a.Artist).Include(c => c.Category);
                    break;
                case "artist_desc":
                    products = products.OrderByDescending(p => p.Artist).Include(a => a.Artist).Include(c => c.Category);
                    break;
                case "Quantity":
                    products = products.OrderByDescending(p => p.Quantity).Include(a => a.Artist).Include(c => c.Category);
                    break;
                case "Click":
                    products = products.OrderByDescending(p => p.Click).Include(a => a.Artist).Include(c => c.Category);
                    break;
                default:
                    products = products.OrderBy(p => p.Name).Include(a => a.Artist).Include(c => c.Category);
                    break;
            }
            
            return View(await products.AsNoTracking().ToListAsync());
            /*
            var theGalleryContext = _context.Product.Include(p => p.Artist).Include(p => p.Category);
            return View(await theGalleryContext.ToListAsync());
            */
        }

        public async Task<IActionResult> Search(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                var products = _context.Product.Where(s => s.Name.Contains(searchString)).Include(a => a.Artist).Include(c => c.Category);
                return View(products);
            }
            else
            {
                var TempContext = _context.Product.Include(p => p.Artist).Include(p => p.Category);
                return View(TempContext.ToList());
            }
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Artist)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            } 
            else
            {
                try
                {
                    // increase click when enter details and update database
                    product.Click = product.Click + 1;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Price,Click,Quantity,Name,ImageURL,Information,ArtistId,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "Name", product.ArtistId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "Name", product.ArtistId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,Click,Quantity,Name,ImageURL,Information,ArtistId,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "Name", product.ArtistId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Artist)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        public JsonResult Search2(string searchString)
        {
            var products = _context.Product.Where(s => s.Name.Contains(searchString));
            
            return Json(products);
        }

    }
}
