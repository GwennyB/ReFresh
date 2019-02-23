using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReFreshMVC.Data;
using ReFreshMVC.Models;
using ReFreshMVC.Models.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IInventoryManager _products;
        public ProductController(IInventoryManager products)
        {
            _products = products;
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
        [HttpPost]
        public async Task<IActionResult> Index(string searchString)
        {
            IEnumerable<Product> products = await _products.GetAllAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString));
            }
            return View(products);
        }

        /// <summary>
        /// displays all meatless products (for non-MeatEaters)
        /// </summary>
        /// <returns> view with all products displayed </returns>
        [HttpGet]
        public async Task<IActionResult> NonMeatProducts()
        {
            IEnumerable<Product> list = await _products.GetAllAsync();
            list = list.Where(s => s.Meaty == false);
            return View("Index", list);

        }

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

    }
}
