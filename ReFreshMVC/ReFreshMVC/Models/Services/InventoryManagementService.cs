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
        private ReFreshDbContext _db { get; }

        public InventoryManagementService(ReFreshDbContext context)
        {
            _db = context;
        }
        
        /// <summary>
        /// Get all Products from DB
        /// </summary>
        /// <returns>List of Products</returns>
        public async Task<List<Product>> GetAllAsync()
        {
            return await _db.Inventory.ToListAsync();
        }
        
        /// <summary>
        /// Get all Non-Meat Products from DB
        /// </summary>
        /// <returns>List of Non-Meats Products</returns>
        public async Task<List<Product>> GetAllNonMeatAsync()
        {
            return await _db.Inventory.Where(p => p.Meaty == false).ToListAsync();
        }
        
        /// <summary>
        /// Get one Product by the ProductID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Product Object</returns>
        public async Task<Product> GetOneByIdAsync(int id)
        {
            return await _db.Inventory.FindAsync(id);
        }
        
        /// <summary>
        /// Update a Product in the DB by the Product Object
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<Product> UpdateAsync(Product product)
        {
            _db.Inventory.Update(product);
            await _db.SaveChangesAsync();
            return await _db.Inventory.FindAsync(product.ID);
        }

        /// <summary>
        /// Add product to DB with Product model
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task CreateAsync(Product product)
        {
            _db.Inventory.Add(product);
            await _db.SaveChangesAsync();
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
                Product productToDelete = await _db.Inventory.FindAsync(id);
                _db.Inventory.Remove(productToDelete);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
