using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReFreshMVC.Models;
using ReFreshMVC.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartManager _cart;
        private readonly UserManager<User> _userManager;

        public CartController(UserManager<User> userManager, ICartManager cart)
        {
            _userManager = userManager;
            _cart = cart;
        }
        public async Task<IActionResult> Index()
        {
            string username = User.Identity.Name;
            Cart cart = await _cart.GetCartAsync(username);

            return View(cart);
        }
    }
}
