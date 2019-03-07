using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReFreshMVC.Models;
using ReFreshMVC.Models.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReFreshMVC.Pages.Admin
{
    public class IndexModel : PageModel
    {
        /// <summary>
        /// create a local context implementing needed interfaces
        /// </summary>
        private readonly ICartManager _cart;
        private IInventoryManager _inv;
        private readonly IEmailSender _mail;
        private UserManager<User> _user;
        public IndexModel(ICartManager cart, IInventoryManager inv, IEmailSender mail, UserManager<User> user)
        {
            _cart = cart;
            _inv = inv;
            _mail = mail;
            _user = user;
        }

        public List<Cart> CartsClosed { get; set; }
        public List<Cart> CartsOpen { get; set; }
        public List<Product> CurrentInventory { get; set; }
        public IList<User> Admins { get; set; }

        /// <summary>
        /// GET: /Admin
        /// populates backing stores and loads Admin Dashboard
        /// </summary>
        /// <returns> Admin Dashboard view </returns>
        public async Task OnGet()
        {
            CartsClosed = await _cart.GetLastTenCarts();
            CartsOpen = await _cart.GetOpenCarts();
            CurrentInventory = await _inv.GetAllAsync();
            Admins = await _user.GetUsersInRoleAsync(AppRoles.Admin);
        }


    }
}