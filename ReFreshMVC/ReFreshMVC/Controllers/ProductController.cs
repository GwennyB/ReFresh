using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReFreshMVC.Data;
using ReFreshMVC.Models;
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
        private readonly UserManager<User> _userManager;

        public ProductController(UserManager<User> userManager, IInventoryManager products, ISearchBarManager search)
        {
            _userManager = userManager;
            _products = products;
            _search = search;
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

        [HttpGet]
        public async Task<IActionResult> Landing(int id) => View(await _products.GetOneByIdAsync(id));
    }
}
