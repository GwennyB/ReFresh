using Microsoft.EntityFrameworkCore;
using ReFreshMVC.Data;
using ReFreshMVC.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models.Services
{
    public class InventoryManagementService : IInventoryManager
    {
        private ReFreshDbContext _context { get; }

        public InventoryManagementService(ReFreshDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Add product to DB with Product model
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task CreateAsync(Product product)
        {
            _context.Inventory.Add(product);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Deletes Product from DB by ProductID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            try
            {
                Product productToDelete = await _context.Inventory.FindAsync(id);
                _context.Inventory.Remove(productToDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Get all Products from DB
        /// </summary>
        /// <returns>List of Products</returns>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Inventory.ToListAsync();
        }
        /// <summary>
        /// Get one Product by the ProductID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Product Object</returns>
        public async Task<Product> GetOneByIdAsync(int id)
        {
            return await _context.Inventory.FindAsync(id);
        }
        /// <summary>
        /// Update a Product in the DB by the Product Object
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Product product)
        {
            _context.Inventory.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
