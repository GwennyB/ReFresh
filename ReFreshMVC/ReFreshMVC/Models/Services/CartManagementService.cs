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
        public async Task<Cart> CreateCartAsync(string user)
        {
            Cart cart = new Cart()
            {
                UserName = user,
                Completed = null
            };
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return await _context.Carts.FirstOrDefaultAsync(c => c.UserName == user && c.Completed == null);
        }

        /// <summary>
        /// closes cart at checkout
        /// applies checkout timestamp to Cart
        /// updates quantities for all items in cart
        /// </summary>
        /// <param name="cart"> cart to close out </param>
        /// <returns> task completed </returns>
        public async Task<bool> CloseCartAsync(Cart cart)
        {
            cart.Completed = DateTime.Now;
            await Task.Run(() => _context.Update(cart));
            //if(cart.Orders != null)
            //{
            //    foreach (Order item in cart.Orders)
            //    {
            //        if (item.Product.QtyAvail < item.Qty)
            //        {
            //            return false;
            //        }
            //        item.Product.QtyAvail -= item.Qty;
            //        await Task.Run(() => _context.Inventory.Update(item.Product));
            //    }
            //}
            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// Gets a user's cart
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Cart</returns>
        public async Task<Cart> GetCartAsync(string username)
        {
            return await _context.Carts.Where(c => c.UserName == username).Include("Orders").FirstOrDefaultAsync();
        }
        /// <summary>
        /// Adds an order with a CartId to the Order Table
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task AddOrderToCart(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
    }
}
