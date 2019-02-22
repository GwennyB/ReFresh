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
        public async Task<IActionResult> Index()
        {
            List<Product> list = await _products.GetAllAsync();
            return View(list);
        }
    }
}
