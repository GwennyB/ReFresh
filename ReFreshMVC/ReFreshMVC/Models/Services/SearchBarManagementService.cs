using Microsoft.EntityFrameworkCore;
using ReFreshMVC.Data;
using ReFreshMVC.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models.Services
{
    public class SearchBarManagementService: ISearchBarManager
    {
        private ReFreshDbContext _context { get; }

        public SearchBarManagementService(ReFreshDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get products based on search string, product category, and meat user claim. 
        /// </summary>
        /// <param name="search"></param>
        /// <param name="category"></param>
        /// <param name="meat"></param>
        /// <returns>IEnumerable<Product>Filtered Products</returns>
        public async Task<IEnumerable<Product>> SearchProducts(string search, int category, bool meat)
        {
            // Get Inventory
            IEnumerable<Product> products = await _context.Inventory.ToListAsync();

            // Filter on Meat Claim
            if (meat == false)
                products = products.Where(p => p.Meaty == false);
            // Filter on enum category 
            if (search == null && category != 10)
                return products.Where(s => (int)s.Category == category);
            // Filter on string search
            if(search != null && category == 10)
                return products.Where(p => p.Name.ToLower().Contains(search.ToLower()));
            // Filter on both enum category and string search
            if(search != null && category != 10)
                return products.Where(p => p.Name.ToLower().Contains(search.ToLower()) && (int)p.Category == category);
            // Return all Meat or Non-Meat products not filtered
            return products;
        }
    }
}
