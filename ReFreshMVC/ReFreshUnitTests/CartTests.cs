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
        public async void CreateCartAsync()
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
        public async void CloseCartAsync()
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

        //[Fact]
        //public async void CloseCartAsync_UpdatesProductQuantities()
        //{
        //    DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("CloseCartAsync").Options;

        //    using (ReFreshDbContext context = new ReFreshDbContext(options))
        //    {
        //        var now = DateTime.Now;
        //        CartManagementService cms = new CartManagementService(context);
        //        InventoryManagementService ims = new InventoryManagementService(context);
        //        await cms.CreateCartAsync("test");
        //        var testCart = await context.Carts.FirstOrDefaultAsync(pr => pr.UserName == "test");
        //        await cms.CloseCartAsync(testCart);
        //        var result = await context.Carts.FirstOrDefaultAsync(pr => pr.ID == testCart.ID);

        //        Assert.Equal(now, result.Completed);
        //    }
        //}

    }
}
