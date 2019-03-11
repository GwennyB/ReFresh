using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReFreshMVC.Models;
using ReFreshMVC.Models.Interfaces;
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
        public IndexModel(ICartManager cart, IInventoryManager inv, IEmailSender mail, UserManager<User> user)
        {
            _cart = cart;
            _inv = inv;
            _mail = mail;
            _user = user;
        }

        public List<Cart> CartsClosed { get; set; }
        public List<Cart> CartsOpen { get; set; }
        public List<Product> CurrentInventory { get; set; }
        public IList<User> Admins { get; set; }

        //[FromRoute]
        //public int? ID { get; set; }
        [BindProperty]
        public Product Product { get; set; }
        //[BindProperty]
        //public IFormFile Image { get; set; }
        //public Blob ImageBlob { get; }

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

            //if (Image != null)
            //{
            //    // do all the blob stuff
            //    // 1. make a filepath
            //    var filePath = Path.GetTempFileName();
            //    // 2. open stream
            //    using (var stream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await Image.CopyToAsync(stream);
            //    }
            //    // 3. get container and blob
            //    var container = await ImageBlob.GetContainer("userpics");
            //    CloudBlob blob = await ImageBlob.GetBlob(Image.FileName, container.Name);
            //    // 4. upload image
            //    ImageBlob.UploadFile(container, Image.FileName, filePath);
            //    query.Photo = blob.Uri.ToString();
            //}

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
                await _inv.UpdateAsync(query);
            }

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