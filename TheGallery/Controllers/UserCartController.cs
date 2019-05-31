using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TheGallery.Models;

namespace TheGallery.Controllers
{
    public class UserCartController : Controller
    {
        private readonly TheGalleryContext _context;

        public UserCartController(TheGalleryContext context)
        {
            _context = context;
        }

        // GET: /UserCart/
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            if (cart != null)
            {
                ViewBag.cart = cart;
                ViewBag.total = cart.Sum(item => item.Product.Price * item.Quantity);
            }
            return View();
        }

        public IActionResult AddToCart(int id)
        {
            // Add to cart, handle null exception and increment quantity if item is already in cart
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                cart = new List<Item>();
                Product product = _context.Product.FindAsync(id).Result;
                cart.Add(new Item { Product = product, Quantity = 1 });
            } else
            {
                int index = isAlreadyInCart(cart, id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                } else
                {
                    Product product = _context.Product.FindAsync(id).Result;
                    cart.Add(new Item { Product = product, Quantity = 1 });
                }
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        // Mock purchase function, removes quantities from stock only without mock pay or anything
        public IActionResult Purchase()
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                var product = cart[i].Product;
                product.Quantity = product.Quantity - cart[i].Quantity;
                // Remove product from database if no stock left
                if (product.Quantity <= 0)
                {
                    _context.Product.Remove(product);
                }
                // Update with new stock
                else
                {
                    _context.Product.Update(product);
                }
            }
            _context.SaveChangesAsync();
            cart = new List<Item>();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return Redirect("~/Products");
        }

        private int isAlreadyInCart(List<Item> cart, int id)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        // Removes all items with same id from the cart
        public IActionResult RemoveFromCart(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = isAlreadyInCart(cart, id);
            if (index != -1)
            {
                cart.RemoveAt(index);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }
    }

    // Helper class for using session variables regarding cart
    public static class SessionHelper
    {
        public static void SetObjectAsJson(ISession session, string key, List<Item> cart)
        {
            session.SetString(key, JsonConvert.SerializeObject(cart));
        }

        public static T GetObjectFromJson<T>(ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}