using Microsoft.AspNetCore.Mvc;
using ReFreshMVC.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace ReFreshMVC.Models.Components
{
    public class ShoppingCart : ViewComponent
    {
        private readonly ICartManager _cart;
        private readonly IInventoryManager _inventory;

        public ShoppingCart(ICartManager cart, IInventoryManager inventory)
        {
            _cart = cart;
            _inventory = inventory;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userID)
        {
            Cart cart = await _cart.GetCartAsync(userID);
            return View(cart);
        }

        [HttpPost]
        public async Task<IViewComponentResult> Edit([Bind("CartID, ProductID, Qty, Product")] Order order)
        {
            Product product = await _inventory.GetOneByIdAsync(order.ProductID);
            Order orderToUpdate = await _cart.GetOrderByCK(order.CartID, order.ProductID);

            orderToUpdate.Qty = order.Qty;
            orderToUpdate.ExtPrice = order.Qty * product.Price;
            await _cart.UpdateOrderInCart(orderToUpdate);
            return View();
        }
    }
}
