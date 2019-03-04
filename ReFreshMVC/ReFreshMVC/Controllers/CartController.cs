using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ReFreshMVC.Models;
using ReFreshMVC.Models.Interfaces;
using System.Text;
using System.Threading.Tasks;

namespace ReFreshMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartManager _cart;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _mail;
        private readonly IInventoryManager _inventory;

        public CartController(UserManager<User> userManager, ICartManager cart, IEmailSender mail , IInventoryManager i)
        {
            _userManager = userManager;
            _cart = cart;
            _mail = mail;
            _inventory = i;
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

        /// <summary>
        /// closes cart and calls /Receipt with closed cart
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            Cart cart = await _cart.GetCartAsync(User.Identity.Name);
            await _cart.CloseCartAsync(cart);
            // add cart to email
            // send email
            return RedirectToAction("Receipt", "Cart", cart);
        }

        /// <summary>
        /// remove an item from the cart
        /// </summary>
        /// <param name="id"> product ID to remove </param>
        /// <returns> loads Cart/Index page </returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string username = User.Identity.Name;

            await _cart.DeleteOrderFromCart(username, id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("CartID, ProductID, Qty, Product")] Order order)
        {
            Product product = await _inventory.GetOneByIdAsync(order.ProductID);
            Order orderToUpdate = await _cart.getOrderByCK(order.CartID, order.ProductID);

            orderToUpdate.Qty = order.Qty;
            orderToUpdate.ExtPrice = order.Qty * product.Price;
            await _cart.UpdateOrderInCart(orderToUpdate);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// sends an email receipt and loads a receipt view
        /// </summary>
        /// <param name="cart"> closed cart to show receipt </param>
        /// <returns> receipt view for cart </returns>
        [HttpGet]
        public async Task<IActionResult> Receipt(Cart cart)
        {
            Cart receipt = await _cart.GetCartByIdAsync(cart.ID);

            // send receipt email
            string subject = "ReFresh Foods Registration";

            StringBuilder message = new StringBuilder();

            message.Append("<p>Thanks for your order! Here's a summary:\n</p>");
            message.Append($"<p>Order #{cart.ID}</p>");
            message.Append($"<p>Order date: {cart.Completed}</p>");
            int total = 0;
            foreach (var order in receipt.Orders)
            {
                message.Append($"<p>{order.Product.Name} (qty {order.Qty}): ${order.ExtPrice}</p>");
                total += order.ExtPrice;
            }
            message.Append($"<p>Order total: ${total}</p>");


            await _mail.SendEmailAsync(User.Identity.Name, subject, message.ToString());

            return View(receipt);
        }
    }
}
