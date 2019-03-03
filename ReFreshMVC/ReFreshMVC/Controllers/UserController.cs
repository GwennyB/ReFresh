using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReFreshMVC.Data;
using ReFreshMVC.Models;
using ReFreshMVC.Models.Interfaces;
using ReFreshMVC.Models.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReFreshMVC.Controllers
{
    public class UserController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private readonly ICartManager _cart;

        /// <summary>
        /// constructs UserController object to manager user account creation and sign-in
        /// </summary>
        /// <param name="userManager"> user manager service context </param>
        /// <param name="signInManager"> signIn manager service context </param>
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, ICartManager cart)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cart = cart;
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
                    Birthdate = bag.Birthdate,
                };

                var query = await _userManager.CreateAsync(user, bag.Password);

                if (query.Succeeded)
                {
                    // define and capture claims
                    Claim fullNameClaim = new Claim("FullName", $"{user.FirstName} {user.LastName}");
                    Claim carnivore = new Claim("Carnivore", $"{bag.EatsMeat}");
                    Claim email = new Claim(ClaimTypes.Email, bag.Email, ClaimValueTypes.Email);

                    // add all claims to DB
                    await _userManager.AddClaimsAsync(user, new List<Claim> { fullNameClaim, carnivore, email });

                    // start a cart for new user
                    await _cart.CreateCartAsync(user.Email);

                    // sign in new user and send to Home/Index
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
                var cart = await _cart.GetCartAsync(bag.Email);
                if (  cart == null)
                {
                    await _cart.CreateCartAsync(bag.Email);
                }

                if (query.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Login failed. Please try again.");

            return View(bag);
        }

        /// <summary>
        /// GET: User/Logout
        /// </summary>
        /// <returns> redirects to Home </returns>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// POST: User/ExtLogin
        /// requests login auth from external provider
        /// </summary>
        /// <param name="provider"> name of OAuth provider </param>
        /// <returns> challenge result </returns>
        [HttpPost]
        public IActionResult ExtLogin(string provider)
        {
            var redirectUrl = Url.Action("ExtLoginCallback", "User");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        /// <summary>
        /// GET: User/ExtLoginCallback
        /// redirect target following external login challenge
        /// </summary>
        /// <param name="error"> external login error status from query string </param>
        /// <returns> home page (or return to login page if unsuccessful) </returns>
        [HttpGet]
        public async Task<IActionResult> ExtLoginCallback(string error = null)
        {
            if (error != null)
            {
                TempData["Error"] = "Oops...external login was unsuccessful. Would you like to try another method?";
                return RedirectToAction("Login");
            }

            var confirm = await _signInManager.GetExternalLoginInfoAsync();

            if (confirm == null)
            {
                return RedirectToAction("Login");
            }

            var login = await _signInManager.ExternalLoginSignInAsync(confirm.LoginProvider, confirm.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (login.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            var email = confirm.Principal.FindFirstValue(ClaimTypes.Email);

            return View("ExtLogin", new ExtLoginViewModel { Email = email });

        }

        /// <summary>
        /// GET: User/ExtLoginConfirmation
        /// confirms login for user, and registers user for local account using OAuth data
        /// </summary>
        /// <param name="bag"> ext user data </param>
        /// <returns> confirmation view with user data </returns>
        [HttpPost]
        public async Task<IActionResult> ExtLoginConfirmation(ExtLoginViewModel bag)
        {
            if (ModelState.IsValid)
            {
                ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    TempData["Error"] = "Oops...external login info didn't load.";
                }

                User user = new User()
                {
                    UserName = bag.Email,
                    Email = bag.Email,
                    FirstName = bag.FirstName,
                    LastName = bag.LastName,
                    Birthdate = bag.Birthdate
                };

                IdentityResult query = await _userManager.CreateAsync(user, bag.Password);

                if (query.Succeeded)
                {
                    // define and capture claims
                    Claim fullNameClaim = new Claim("FullName", $"{user.FirstName} {user.LastName}");
                    Claim carnivore = new Claim("Carnivore", $"{bag.EatsMeat}");
                    Claim email = new Claim(ClaimTypes.Email, bag.Email, ClaimValueTypes.Email);

                    // add all claims to DB
                    await _userManager.AddClaimsAsync(user, new List<Claim> { fullNameClaim, carnivore, email });

                    // create external login association
                    await _userManager.AddLoginAsync(user, info);

                    // start a cart for new user
                    await _cart.CreateCartAsync(bag.Email);

                    // sign in new user and send to Home/Index
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");

                }

            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Privacy() => View();
    }

       
}
