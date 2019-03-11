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

        [HttpGet]
        public async Task<IViewComponentResult> InvokeAsync() => View(await _cart.GetCartAsync(User.Identity.Name));

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
