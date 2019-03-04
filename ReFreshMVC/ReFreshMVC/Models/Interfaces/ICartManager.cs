using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models.Interfaces
{
    public interface ICartManager
    {
        Task<Cart> CreateCartAsync(string user);
        Task<bool> CloseCartAsync(Cart cart);
        Task<Cart> GetCartAsync(string username);

        Task AddOrderToCart(Order order);
        Task UpdateOrderInCart(Order order);
        Task DeleteOrderFromCart(string username, int productId);
        Task<Cart> GetCartByIdAsync(int cartId);
        Task<Order> GetOrderByCK(int cartId, int productId);
    }
}
