using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using AuthorizeNet.Api.Contracts.V1;
using ReFreshMVC.Models;
using ReFreshMVC.Models.Interfaces;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ReFreshMVC.Models.ViewModels;

namespace ReFreshMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartManager _cart;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _mail;
        private readonly IInventoryManager _inventory;
        private readonly IAuthorizeNetManager _payment;

        public CartController(UserManager<User> userManager, IAuthorizeNetManager payment, ICartManager cart, IEmailSender mail, IInventoryManager inventory)
        {
            _userManager = userManager;
            _cart = cart;
            _mail = mail;
            _inventory = inventory;
            _payment = payment;
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
            //if(TempData["paymentResponse"]. != null)
            //{
            //    ViewBag["paymentResponse"] = TempData["paymentResponse"];
            //}
            return View(cart);
        }

        /// <summary>
        /// closes cart and calls /Receipt with closed cart
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Checkout([Bind("Number, ExpDate")] CreditCardViewModel ccvm)
        {
            Cart cart = await _cart.GetCartAsync(User.Identity.Name);

            // Get cart total
            int amount = 0;
            foreach(Order o in cart.Orders)
            {
                amount += o.ExtPrice;
            }

            createTransactionResponse response = _payment.RunCard(amount, ccvm.ExpDate, ccvm.Number);
            if (response.messages.resultCode == messageTypeEnum.Ok)
            {
                await _cart.CloseCartAsync(cart);
                return RedirectToAction("Receipt", "Cart", cart);
            }
            else
            {
                TempData["paymentResponse"] = response.messages.resultCode;
                return RedirectToAction("Index", "Cart");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            Cart cart = await _cart.GetCartAsync(User.Identity.Name);

            return View(cart);
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

        /// <summary>
        /// Post: Order
        /// updates an order quantity
        /// </summary>
        /// <param name="order"></param>
        /// <returns>Cart View</returns>
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("CartID, ProductID, Qty, Product")] Order order)
        {
            Product product = await _inventory.GetOneByIdAsync(order.ProductID);
            Order orderToUpdate = await _cart.GetOrderByCK(order.CartID, order.ProductID);

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
            string subject = "ReFresh Foods Order Confirmation";

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
