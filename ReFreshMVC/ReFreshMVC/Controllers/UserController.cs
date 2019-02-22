﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReFreshMVC.Data;
using ReFreshMVC.Models;
using ReFreshMVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
                    Claim fullNameClaim = new Claim("FullName", $"{user.FirstName} {user.LastName}");
                    await _userManager.AddClaimsAsync(user, new List<Claim> { fullNameClaim });

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(bag);
        }

        /// <summary>
        /// GET: User/Login
        /// loads login page
        /// </summary>
        /// <returns> login view </returns>
        [HttpGet]
        public IActionResult Login() => View();

        /// <summary>
        /// POST: User/Login
        /// redirects to Home if login successful, reloads login page with entered data if unsuccessful
        /// </summary>
        /// <param name="bag"> login data submitted by user </param>
        /// <returns> View (home or login) </returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel bag)
        {
            if (ModelState.IsValid)
            {
                var query = await _signInManager.PasswordSignInAsync(bag.Email, bag.Password, false, false);

                if (query.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Login failed. Please try again.");

            return View(bag);
        }

    }


}