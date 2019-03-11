using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReFreshMVC.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// GET: Home/Index
        /// displays splash page
        /// </summary>
        /// <returns> Homepage view </returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index() => View();

    }
}
