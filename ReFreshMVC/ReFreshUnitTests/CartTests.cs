using System;
using Xunit;
using ReFreshMVC.Models;
using ReFreshMVC.Models.Services;
using ReFreshMVC.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ReFreshUnitTests
{
    public class CartTests
    {
        [Fact]
        public void IDGetSet()
        {
            Cart p = new Cart();
            p.ID = 1;
            Assert.Equal(1,p.ID);
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
        public async void CanCreateCart()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("CreateCartAsync").Options;

            using(ReFreshDbContext context = new ReFreshDbContext(options))
            {
                var now = DateTime.Now;

                CartManagementService cms = new CartManagementService(context);
                await cms.CreateCartAsync("test");
                var result = context.Carts.FirstOrDefaultAsync(pr => pr.UserName == "test");

                Assert.Equal("test", result.Result.UserName);
            }
        }

        [Fact]
        public async void CanCloseCart()
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
        public async void CanGetCart()
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
        public async void CanAddOrderToCart()
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
        public async void CanUpdateOrderInCart()
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

                order.ExtPrice = 30;

                await cms.UpdateOrderInCart(order);

                var result = await context.Orders.FirstOrDefaultAsync(o => o.CartID == 1 && o.ProductID == 1);

                Assert.Equal(30, result.ExtPrice);
            }
        }

        [Fact]
        public async void CanRemoveOrderFromCart()
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
                await cms.DeleteOrderFromCart("test", 1);
                var result = await context.Orders.FirstOrDefaultAsync(o => o.CartID == 1 && o.ProductID == 1);

                Assert.Null(result);
            }
        }

        [Fact]
        public async void CanGetCartByID()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("GetCartAsync").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                CartManagementService cms = new CartManagementService(context);
                await cms.CreateCartAsync("test");

                var result = await cms.GetCartByIdAsync(1);

                Assert.Equal("test", result.UserName);
            }
        }

        [Fact]
        public async void CanGetOrderByCK()
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

                var result = await cms.GetOrderByCK(1,1);

                Assert.Equal(25,result.ExtPrice);
            }
        }
    }
}
