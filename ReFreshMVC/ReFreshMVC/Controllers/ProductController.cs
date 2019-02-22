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
    //[Authorize(Policy = "LoggedIn")]
    public class ProductController : Controller
    {
        private readonly IInventoryManager _products;
        public ProductController(IInventoryManager products)
        {
            _products = products;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> list = await _products.GetAllAsync();
            return View(list);
        }

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
    }
}
