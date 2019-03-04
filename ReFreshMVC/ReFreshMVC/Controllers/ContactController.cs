using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Controllers
{
    public class ContactController: Controller
    {
        /// <summary>
        /// GET: Contact/PrivacyPolicy
        /// returns privacy policy page
        /// </summary>
        /// <returns> PrivacyPolicy view </returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}
