using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReFreshMVC.Data;
using ReFreshMVC.Models;
using ReFreshMVC.Models.ViewModels;
using ReFreshMVC.Models.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReFreshMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IInventoryManager _products;
        private readonly ISearchBarManager _search;
        private readonly ICartManager _cart;
        private readonly UserManager<User> _userManager;

        public ProductController(UserManager<User> userManager, IInventoryManager products, ISearchBarManager search, ICartManager cart)
        {
            _userManager = userManager;
            _products = products;
            _search = search;
            _cart = cart;
        }

        /// <summary>
        /// displays all products (restricted to 'Carnivore' users only)
        /// </summary>
        /// <returns> view with all products displayed </returns>
        [Authorize(Policy = "Carnivore")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> list = await _products.GetAllAsync();
            return View(list);
        }

        /// <summary>
        /// displays all products matching the search string
        /// </summary>
        /// <param name="searchString"> string to search </param>
        /// <returns> view with filtered products list </returns>
        [Authorize(Policy = "Carnivore")]
        [HttpPost]
        public async Task<IActionResult> Index(string searchString, int searchCategory) => View(await _search.SearchProducts(searchString, searchCategory, true));

        /// <summary>
        /// displays all meatless products (for non-MeatEaters)
        /// </summary>
        /// <returns> view with all products displayed </returns>
        [HttpGet]
        public async Task<IActionResult> NonMeatProducts() => View("Index", await _products.GetAllNonMeatAsync());

        /// <summary>
        /// displays all products matching the search string (for non-MeatEaters)
        /// </summary>
        /// <param name="searchString"> string to search </param>
        /// <returns> view with filtered products list </returns>
        [HttpPost]
        public async Task<IActionResult> NonMeatProducts(string searchString, int searchCategory) => View("Index", await _search.SearchProducts(searchString, searchCategory, false));

        /// <summary>
        /// captures 404 on this route (from non-MeatEater 'AccessDenied') and redirects to NonMeatProducts
        /// </summary>
        /// <returns> redirect to NonMeatProducts route </returns>
        // error handling thanks to https://www.devtrends.co.uk/blog/handling-404-not-found-in-asp.net-core
        [Route("error/404")]
        public IActionResult Error404()
        {
            return RedirectToAction("NonMeatProducts", "Product");
        }
        /// <summary>
        /// displays a list of all products to the Iventory View
        /// </summary>
        /// <returns>View of Product List</returns>
        [HttpGet]
        public async Task<IActionResult> Inventory()
        {
            IEnumerable<Product> list = await _products.GetAllAsync();
            return View(list);
        }
        /// <summary>
        /// displays creation page for Inventory
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create() => View();
        /// <summary>
        /// displays landing page for product by product id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View with product details</returns>
        [HttpGet]
        public async Task<IActionResult> Landing(int id)
        {
            Product product = await _products.GetOneByIdAsync(id);

            OrderProductViewModel opvm = new OrderProductViewModel();
            opvm.ProductID = product.ID;
            opvm.Sku = product.Sku;
            opvm.Name = product.Name;
            opvm.Price = product.Price;
            opvm.Description = product.Description;
            opvm.QtyAvail = product.QtyAvail;
            opvm.Image = product.Image;
            opvm.Category = product.Category;
            opvm.Meaty = product.Meaty;

            return View(opvm);
        } 
        [HttpPost]
        public async Task<IActionResult> AddToCart([Bind("ProductID, Qty, ExtPrice")] Order order)
        {
            string username = User.Identity.Name;
            Cart cart = await _cart.GetCartAsync(username);
            Product product = await _products.GetOneByIdAsync(order.ProductID);

            // Order object complete here
            order.CartID = cart.ID;
            order.ExtPrice = order.Qty * product.Price;

            // Check if order exists
            
            if (cart.Orders.Where(o => o.CartID == cart.ID && o.ProductID == order.ProductID).FirstOrDefault() != null)
            {
                await _cart.UpdateOrderInCart(order);
            }
            else
            {
                await _cart.AddOrderToCart(order);
            }
            return RedirectToAction("Index");
        }
    }
}
