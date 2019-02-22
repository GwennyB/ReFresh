using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// GET Action for homepage
        /// </summary>
        /// <returns>Homepage view</returns>
        public IActionResult Index()
        {
            return View();
        }

    }
}
