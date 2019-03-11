using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReFreshMVC.Models.Interfaces;
using ReFreshMVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models.Components
{
    public class Payment : ViewComponent
    {
        private readonly ICartManager _cart;
        private readonly IInventoryManager _inventory;

        public Payment(ICartManager cart, IInventoryManager inventory)
        {
            _cart = cart;
            _inventory = inventory;
        }
        /// <summary>
        /// Get: Payment Modal View
        /// </summary>
        /// <returns>Payment Modal View with CreditCardViewModel</returns>
        [HttpGet]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            CreditCardViewModel ccvm = new CreditCardViewModel();
            ccvm.Cart = await _cart.GetCartAsync(User.Identity.Name);

            List<string> creditCardNumbers = new List<string>();
            creditCardNumbers.Add("370000000000002");
            creditCardNumbers.Add("6011000000000012");
            creditCardNumbers.Add("4007000000027");
            creditCardNumbers.Add("4111111111111111");

            ViewData["CreditCardNumbers"] = new SelectList(creditCardNumbers);
            
            return View(ccvm);
        }
    }
}
