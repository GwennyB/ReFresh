using Microsoft.AspNetCore.Mvc.RazorPages;
using ReFreshMVC.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using ReFreshMVC.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Collections.Generic;

namespace ReFreshMVC.Pages.Admin
{
    public class IndexModel : PageModel
    {
        /// <summary>
        /// create a local context implementing needed interfaces
        /// </summary>
        private readonly IInventoryManager _inv;
        private readonly ICartManager _cart;
        private readonly IEmailSender _mail;
        private UserManager<User> _user;
        public IndexModel(IInventoryManager inv, ICartManager cart, UserManager<User> user, IEmailSender mail)
        {
            _inv = inv;
            _cart = cart;
            _user = user;
            _mail = mail;
        }


        public List<Cart> CartsClosed { get; set; }
        public List<Cart> CartsOpen { get; set; }

        public async void OnGet()
        {
            CartsClosed = await _cart.GetLastTenCarts();
            CartsOpen = await _cart.GetOpenCarts();
        }
    }
}