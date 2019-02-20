using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReFreshMVC.Data;
using ReFreshMVC.Models;
using ReFreshMVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Controllers
{
    public class UserController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        /// <summary>
        /// constructs UserController object to manager user account creation and sign-in
        /// </summary>
        /// <param name="userManager"> user manager service context </param>
        /// <param name="signInManager"> signIn manager service context </param>
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// GET: User/Register
        /// loads user registration page
        /// </summary>
        /// <returns> blank registration page </returns>
        [HttpGet]
        public IActionResult Register() => View();

        /// <summary>
        /// POST: User/Register
        /// submits registration data
        /// redirects home if successful
        /// stays on Reg page (with user data loaded) until successful
        /// </summary>
        /// <param name="bag"> data submitted by user on Registration form </param>
        /// <returns> Home page if successful account creation, Registration page (with sent data) if not </returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel bag)
        {
            if(ModelState.IsValid)
            {
                User user = new User()
                {
                    UserName = bag.Email,
                    Email = bag.Email,
                    FirstName = bag.FirstName,
                    LastName = bag.LastName,
                    Birthdate = bag.Birthdate
                };

                var query = await _userManager.CreateAsync(user, bag.Password);

                if(query.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(bag);
        }

    }


}
