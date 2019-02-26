using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models.Interfaces
{
    public interface IInventoryManager
    {
        
        Task CreateAsync(Product product);
        
        Task<List<Product>> GetAllAsync();

        Task<List<Product>> GetAllNonMeatAsync();

        Task<Product> GetOneByIdAsync(int id);
        
        Task UpdateAsync(Product product);
        
        Task DeleteAsync(int id);


    }
}
