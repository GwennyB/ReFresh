using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReFreshMVC.Data;
using ReFreshMVC.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models.Services
{
    public class CartManagementService : ICartManager
    {
        private ReFreshDbContext _context { get; }

        public CartManagementService(ReFreshDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// creates a new cart for currently logged in user
        /// </summary>
        /// <param name="username"> FullName claim of logged in user </param>
        /// <returns> newly created cart </returns>
        public async Task<Cart> CreateCartAsync(string username)
        {
            Cart cart = new Cart()
            {
                UserName = username,
                Completed = null
            };
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return await _context.Carts.FirstOrDefaultAsync(c => c.UserName == username && c.Completed == null);
        }

        /// <summary>
        /// closes cart at checkout
        /// applies checkout timestamp to Cart
        /// updates quantities for all items in cart
        /// </summary>
        /// <param name="cart"> cart to close out </param>
        /// <returns> task completed </returns>
        public async Task CloseCart(Cart cart)
        {
            cart.Completed = DateTime.Now;
            await Task.Run(() => _context.Update(cart));
            foreach (Order item in cart.Orders)
            {
                item.Product.QtyAvail -= item.Qty;
                await Task.Run(() => _context.Inventory.Update(item.Product));
            }
            await _context.SaveChangesAsync();
        }


    }
}
