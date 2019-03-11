using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using ReFreshMVC.Models;
using ReFreshMVC.Models.Interfaces;
using ReFreshMVC.Models.Util;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ReFreshMVC.Pages.Admin
{
    [Authorize("AdminOnly")]
    public class IndexModel : PageModel
    {
        /// <summary>
        /// create a local context implementing needed interfaces
        /// </summary>
        private readonly ICartManager _cart;
        private IInventoryManager _inv;
        private readonly IEmailSender _mail;
        private UserManager<User> _user;
        public Blob ImageBlob { get; }
        public IndexModel(ICartManager cart, IInventoryManager inv, IEmailSender mail, UserManager<User> user, IConfiguration config)
        {
            _cart = cart;
            _inv = inv;
            _mail = mail;
            _user = user;
            ImageBlob = new Blob(config);

        }

        public List<Cart> CartsClosed { get; set; }
        public List<Cart> CartsOpen { get; set; }
        public List<Product> CurrentInventory { get; set; }
        public IList<User> Admins { get; set; }

        //[FromRoute]
        //public int? ID { get; set; }
        [BindProperty]
        public Product Product { get; set; }
        [BindProperty]
        public IFormFile Image { get; set; }

        [BindProperty]
        public string Email { get; set; }

        /// <summary>
        /// GET: /Admin
        /// populates backing stores and loads Admin Dashboard
        /// </summary>
        /// <returns> Admin Dashboard view </returns>
        public async Task OnGet()
        {
            CartsClosed = await _cart.GetLastTenCarts();
            CartsOpen = await _cart.GetOpenCarts();
            CurrentInventory = await _inv.GetAllAsync();
            Admins = await _user.GetUsersInRoleAsync(AppRoles.Admin);
        }

        /// <summary>
        /// updates existing inventory entry or creates a new one
        /// </summary>
        /// <returns> reloads Admin page </returns>
        public async Task<IActionResult> OnPost()
        {
            Product query = await _inv.GetOneByIdAsync(Product.ID);
            Product.ID = 0;

            if (Image != null)
            {
                // do all the blob stuff
                // 1. make a filepath
                var filePath = Path.GetTempFileName();
                // 2. open stream
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }
                // 3. get container and blob
                var container = await ImageBlob.GetContainer("products");
                CloudBlob blob = await ImageBlob.GetBlob(Image.FileName, container.Name);
                // 4. upload image
                ImageBlob.UploadFile(container, Image.FileName, filePath);
                Product.Image = blob.Uri.ToString();
            }

            if (query == null || query.ID == 0)
            {
                await _inv.CreateAsync(Product);
            }
            else
            {
                query.Sku = Product.Sku;
                query.Name = Product.Name;
                query.Price = Product.Price;
                query.QtyAvail = Product.QtyAvail;
                query.Description = Product.Description;
                query.Meaty = Product.Meaty;
                query.Category = Product.Category;
                query.Image = Product.Image;
                await _inv.UpdateAsync(query);
            }
            CurrentInventory = await _inv.GetAllAsync();
            return RedirectToPage("../Admin/Index");
        }

        public async Task<IActionResult> OnPostDelete()
        {
            await _inv.DeleteAsync(Product.ID);
            return RedirectToPage("../Admin/Index");
        }

        public async Task<IActionResult> OnPostMakeAdmin()
        {
            User user = await _user.FindByEmailAsync(Email);
            if (user != null)
            {
                await _user.AddToRoleAsync(user, AppRoles.Admin);
            }
            return RedirectToPage("../Admin/Index");
        }

    }
}