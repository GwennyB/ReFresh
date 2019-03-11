using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models.Interfaces
{
    public interface ISearchBarManager
    {
        Task<IEnumerable<Product>> SearchProducts(string search, int category, bool meat);
    }
}
