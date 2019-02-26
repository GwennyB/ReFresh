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
        public async Task<IEnumerable<Product>> SearchProducts(string search, int category, bool meat)
        {
            IEnumerable<Product> products = await _context.Inventory.ToListAsync();
            if (meat == false)
                products = products.Where(p => p.Meaty == false);

            if (search == "" && category == 10)
                return products;
            if (search == "" && category != 10)
                return products.Where(s => (int)s.Category == category);
            if(search != "" && category == 10)
                return products.Where(p => p.Name.ToLower().Contains(search.ToLower()));
            if(search != "" && category != 10)
                return products.Where(p => p.Name.ToLower().Contains(search.ToLower()) && (int)p.Category == category);
            return products;
        }
    }
}
