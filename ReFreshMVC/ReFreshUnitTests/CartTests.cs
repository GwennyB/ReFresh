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
        public async void CreateAsync()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("CreateCart").Options;

            using(ReFreshDbContext context = new ReFreshDbContext(options))
            {
                Cart testCart = new Cart();
                testCart.Sku = 1;
                testCart.Name = "Test Cart";
                testCart.Price = 5;
                testCart.Description = "This is a test";
                testCart.Image = "https://image-url.com";

                InventoryManagementService ims = new InventoryManagementService(context);
                await ims.CreateAsync(testCart);
                var result = context.Inventory.FirstOrDefaultAsync(pr => pr.ID == testCart.ID);

                Assert.True(testCart.Sku == result.Result.Sku);
            }
        }
        [Fact]
        public async void DeleteAsync()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("DeleteCart").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                Cart testCart = new Cart();
                testCart.Sku = 1;
                testCart.Name = "Test Cart";
                testCart.Price = 5;
                testCart.Description = "This is a test";
                testCart.Image = "https://image-url.com";

                InventoryManagementService ims = new InventoryManagementService(context);
                await ims.CreateAsync(testCart);
                await ims.DeleteAsync(testCart.ID);

                await Assert.ThrowsAsync<ArgumentNullException> (()=> ims.DeleteAsync(testCart.ID));
            }
        }
        [Fact]
        public async void GetAllAsync()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("GetAllCart").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                Cart testCart = new Cart();
                testCart.Sku = 1;
                testCart.Name = "Test Cart";
                testCart.Price = 5;
                testCart.Description = "This is a test";
                testCart.Image = "https://image-url.com";

                InventoryManagementService ims = new InventoryManagementService(context);
                await ims.CreateAsync(testCart);

                List<Cart> result = await ims.GetAllAsync() as List<Cart>;
                Assert.True(result.Count == 1);
            }
        }
        [Fact]
        public async void GetOneAsync()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("GetOneCart").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                Cart testCart = new Cart();
                testCart.Sku = 1;
                testCart.Name = "Test Cart";
                testCart.Price = 5;
                testCart.Description = "This is a test";
                testCart.Image = "https://image-url.com";

                InventoryManagementService ims = new InventoryManagementService(context);
                await ims.CreateAsync(testCart);

                Cart result = ims.GetOneByIdAsync(testCart.ID).Result;
                Assert.True(result.ID == testCart.ID);
            }
        }
        [Fact]
        public async void UpdateAsync()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("UpdateCart").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                Cart testCart = new Cart();
                testCart.ID = 1;
                testCart.Sku = 1;
                testCart.Name = "Test Cart";
                testCart.Price = 5;
                testCart.Description = "This is a test";
                testCart.Image = "https://image-url.com";

                InventoryManagementService ims = new InventoryManagementService(context);
                await ims.CreateAsync(testCart);

                Cart updateCart = testCart;
                updateCart.Name = "Update Cart";

                await ims.UpdateAsync(updateCart);
                Cart p = await ims.GetOneByIdAsync(testCart.ID);

                Assert.True(p.Name == "Update Cart");
            }
        }
    }
}
