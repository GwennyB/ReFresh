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

        /// <summary>
        /// GET: Cart/Index
        /// loads cart for valid user; redirects to login when no valid user
        /// </summary>
        /// <returns> Cart view (with cart items) or Login view </returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string username = User.Identity.Name;

            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }
            
            Cart cart = await _cart.GetCartAsync(username);
            if (cart == null)
            {
                cart = await _cart.CreateCartAsync(username);
            }
            return View(cart);
        }
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            Cart cart = await _cart.GetCartAsync(User.Identity.Name);
            await _cart.CloseCartAsync(cart);
            // add cart to email
            // send email
            return RedirectToAction("Index", "Home");
        }
    }
}
