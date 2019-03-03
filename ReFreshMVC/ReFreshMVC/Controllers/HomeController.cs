﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// GET: Home/Index
        /// displays splash page
        /// </summary>
        /// <returns> Homepage view </returns>
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

    }
}
