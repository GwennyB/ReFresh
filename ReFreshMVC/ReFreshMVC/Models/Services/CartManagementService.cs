﻿using Microsoft.AspNetCore.Mvc;
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
            var query = await _context.Carts.FirstOrDefaultAsync(c => c.UserName == user && c.Completed == null);
            return query;

        }

        /// <summary>
        /// closes cart at checkout
        /// applies checkout timestamp to Cart
        /// updates quantities for all items in cart
        /// </summary>
        /// <param name="cart"> cart to close out </param>
        /// <returns> task completed </returns>
        public async Task CloseCartAsync(Cart cart)
        {
            cart.Completed = DateTime.Now;
            await Task.Run(() => _context.Update(cart));
            await _context.SaveChangesAsync();
            cart.Orders = await _context.Orders.Where(o => o.CartID == cart.ID).ToListAsync();
            if (cart.Orders != null)
            {
                foreach (Order item in cart.Orders)
                {
                    if (item.Product.QtyAvail < item.Qty)
                    {
                        item.Qty = item.Product.QtyAvail;
                    }
                    item.Product.QtyAvail -= item.Qty;
                    await Task.Run(() => _context.Inventory.Update(item.Product));
                }
            }
            await _context.SaveChangesAsync();
            await CreateCartAsync(cart.UserName);
        }

        /// <summary>
        /// Gets a user's cart
        /// </summary>
        /// <param name="username"></param>
        /// <returns> open Cart belonging to 'username' </returns>
        public async Task<Cart> GetCartAsync(string username)
        {
            Cart cart = await _context.Carts.Where(c => c.UserName == username && c.Completed == null).Include("Orders.Product").FirstOrDefaultAsync();


            if(cart != null)
            {
                cart.Total = 0;
                foreach (Order order in cart.Orders)
                    cart.Total += order.ExtPrice;

                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();
            }
            return cart;
        }

        /// <summary>
        /// Adds an order with a CartId to the Order Table
        /// </summary>
        /// <param name="order"> order object to add </param>
        /// <returns> task completed </returns>
        public async Task AddOrderToCart(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// updates an item in cart
        /// </summary>
        /// <param name="order"> item in cart to update </param>
        /// <returns> completed task </returns>
        public async Task UpdateOrderInCart(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// removes an item from cart
        /// </summary>
        /// <param name="username"> cart owner's email address </param>
        /// <param name="productId"> Product Id property </param>
        /// <returns> completed task </returns>
        public async Task DeleteOrderFromCart(string username, int productId)
        {
            Cart cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserName == username && c.Completed == null);
            Order order = await _context.Orders.FirstOrDefaultAsync(o => o.CartID == cart.ID && o.ProductID == productId);
            if(cart != null && order != null)
            {
                _context.Orders.Remove(order);
            }
            await _context.SaveChangesAsync();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cartId">Cart Id property</param>
        /// <returns>Cart Object</returns>
        public async Task<Cart> GetCartByIdAsync(int cartId)
        {
            Cart cart = await _context.Carts.Where(c => c.ID == cartId).Include("Orders.Product").FirstOrDefaultAsync();
            foreach(Order order in cart.Orders)
                    cart.Total += order.ExtPrice;

            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        /// <summary>
        /// Get: Order
        /// finds order by CartID and ProductID
        /// </summary>
        /// <param name="cartId">Cart Id property</param>
        /// <param name="productId">Product Id property</param>
        /// <returns>Order order</returns>
        public async Task<Order> GetOrderByCK(int cartId, int productId) => await _context.Orders.Where(o => o.CartID == cartId && o.ProductID == productId).FirstOrDefaultAsync();

        /// <summary>
        /// queries Carts table for last 10 'closed' carts
        /// </summary>
        /// <returns> List of closed carts </returns>
        public async Task<List<Cart>> GetLastTenCarts()
        {
            List<Cart> carts = await _context.Carts.OrderByDescending(c => c.Completed).Take(10).ToListAsync();
            foreach (Cart cart in carts)
            {
                if (cart.Orders != null)
                {
                    foreach (Order item in cart.Orders)
                    {
                        cart.Total += item.ExtPrice;
                    }
                }
            }
            return carts;
        }

        /// <summary>
        /// queries Carts table for all 'open' carts
        /// </summary>
        /// <returns> List of open carts </returns>
        public async Task<List<Cart>> GetOpenCarts()
        {
            List<Cart> carts = await _context.Carts.Where(c => c.Completed == null).Include("Orders.Product").ToListAsync();
            foreach (Cart cart in carts)
            {
                if (cart.Orders != null)
                {
                    foreach (Order item in cart.Orders)
                    {
                        cart.Total += item.ExtPrice;
                    }
                }
            }
            return carts;
        }

        /// <summary>
        /// update the database with a cart object
        /// </summary>
        /// <param name="cart">Cart object</param>
        /// <returns> task completed </returns>
        public async Task UpdateCart(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }
    }
}
