using System;
using Xunit;
using ReFreshMVC.Models;
using ReFreshMVC.Models.Services;
using ReFreshMVC.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReFreshUnitTests
{
    public class CartTests
    {
        [Fact]
        public void IDGetSet()
        {
            Cart p = new Cart();
            p.ID = 1;
            Assert.Equal(1, p.ID);
        }

        [Fact]
        public void UserNameGetSet()
        {
            Cart p = new Cart();
            p.UserName = "test";
            Assert.Equal("test", p.UserName);
        }

        [Fact]
        public void CompletedGetSet()
        {
            Cart p = new Cart();
            var now = DateTime.Now;
            p.Completed = now;
            Assert.Equal(now, p.Completed);
        }

        [Fact]
        public async Task CanCreateCart()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("CreateCartAsync").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                var now = DateTime.Now;

                CartManagementService cms = new CartManagementService(context);
                await cms.CreateCartAsync("test");
                var result = context.Carts.FirstOrDefaultAsync(pr => pr.UserName == "test");

                Assert.Equal("test", result.Result.UserName);
            }
        }

        [Fact]
        public async Task CanCloseCart()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("CloseCartAsync").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                CartManagementService cms = new CartManagementService(context);
                await cms.CreateCartAsync("test cart");
                var testCart = await context.Carts.FirstOrDefaultAsync(pr => pr.UserName == "test cart");
                await cms.CloseCartAsync(testCart);
                var result = await context.Carts.FirstOrDefaultAsync(pr => pr.UserName == "test cart");

                Assert.NotNull(result.Completed);
            }
        }

        [Fact]
        public async Task CanGetCart()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("GetCartAsync").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                CartManagementService cms = new CartManagementService(context);
                await cms.CreateCartAsync("test");

                var result = await cms.GetCartAsync("test");

                Assert.Equal("test", result.UserName);
            }
        }

        [Fact]
        public async Task CanAddOrderToCart()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("AddOrderToCart").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                CartManagementService cms = new CartManagementService(context);
                await cms.CreateCartAsync("test");
                Order order = new Order();
                order.CartID = 1;
                order.ProductID = 1;
                order.Qty = 5;
                order.ExtPrice = 25;
                await cms.AddOrderToCart(order);

                var result = await context.Orders.FirstOrDefaultAsync(o => o.CartID == 1 && o.ProductID == 1);

                Assert.Equal(25, result.ExtPrice);
            }
        }

        [Fact]
        public async Task CanDeleteOrderFromCart()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("RemoveOrderFromCart").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                CartManagementService cms = new CartManagementService(context);
                var query = await cms.CreateCartAsync("test");
                Order order = new Order();
                order.CartID = 1;
                order.ProductID = 1;
                order.Qty = 5;
                order.ExtPrice = 25;
                await cms.AddOrderToCart(order);
                await cms.DeleteOrderFromCart("test", 1);

                Assert.Null(await context.Orders.FirstOrDefaultAsync(o => o.CartID == 1 && o.ProductID == 1));

            }
        }

        [Fact]
        public async Task CanGetCartByID()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("GetCartByID").Options;
            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                CartManagementService cms = new CartManagementService(context);
                Cart cart = new Cart()
                {
                    UserName = "test",
                    Completed = null
                };
                await context.Carts.AddAsync(cart);
                await context.SaveChangesAsync();

                var result = await cms.GetCartByIdAsync(cart.ID);
                Assert.Equal("test", result.UserName);
            }
        }

        [Fact]
        public async Task CanUpdateOrderInCart()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("UpdateOrderInCart").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                CartManagementService cms = new CartManagementService(context);
                await cms.CreateCartAsync("test");
                Order order = new Order();
                order.CartID = 1;
                order.ProductID = 1;
                order.Qty = 5;
                order.ExtPrice = 25;
                await cms.AddOrderToCart(order);

                order.ExtPrice = 30;

                await cms.UpdateOrderInCart(order);

                var result = await context.Orders.FirstOrDefaultAsync(o => o.CartID == 1 && o.ProductID == 1);

                Assert.Equal(30, result.ExtPrice);
            }
        }

        [Fact]
        public async Task CanGetOrderByCK()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("GetOrderByCK").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                CartManagementService cms = new CartManagementService(context);
                await cms.CreateCartAsync("test");
                Order order = new Order();
                order.CartID = 1;
                order.ProductID = 1;
                order.Qty = 5;
                order.ExtPrice = 25;
                await cms.AddOrderToCart(order);

                var result = await cms.GetOrderByCK(1, 1);

                Assert.Equal(25, result.ExtPrice);
            }
        }

        [Fact]
        public async Task CanGetLastTen()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("GetLastTen").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                CartManagementService cms = new CartManagementService(context);
                List<Cart> carts = new List<Cart>();
                for(int i = 0; i < 10; i++)
                {
                    await cms.CreateCartAsync($"test{i}");
                    carts.Add(await cms.GetCartAsync($"test{i}"));
                }

                var result = await cms.GetLastTenCarts();

                Assert.Equal(carts, result);
            }
        }

        [Fact]
        public async Task CanGetOpenCarts()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("GetLastTen").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                CartManagementService cms = new CartManagementService(context);
                List<Cart> carts = new List<Cart>();
                for (int i = 1; i <= 5; i++)
                {
                    await cms.CreateCartAsync($"test{i}");
                    carts.Add(await cms.GetCartAsync($"test{i}"));
                }
                for (int i = 6; i <= 10; i++)
                {
                    await cms.CreateCartAsync($"test{i}");
                    Cart cart = await cms.GetCartAsync($"test{i}");
                    await cms.CloseCartAsync(cart);
                    carts.Add(await cms.GetCartAsync($"test{i}"));
                }

                var result = await cms.GetOpenCarts();

                Assert.Equal(carts, result);
            }
        }
    }
}
