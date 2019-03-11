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
    public class ProductTests
    {
        [Fact]
        public void IDGetSet()
        {
            Product p = new Product();
            p.ID = 1;
            Assert.True(p.ID == 1);
        }
        [Fact]
        public void SkuGetSet()
        {
            Product p = new Product();
            p.Sku = 1;
            Assert.True(p.Sku == 1);
        }
        [Fact]
        public void NameGetSet()
        {
            Product p = new Product();
            p.Name = "name";
            Assert.True(p.Name == "name");
        }
        [Fact]
        public void PriceGetSet()
        {
            Product p = new Product();
            p.Price = 5;
            Assert.True(p.Price == 5);
        }
        [Fact]
        public void DescriptionGetSet()
        {
            Product p = new Product();
            p.Description = "Test";
            Assert.True(p.Description == "Test");
        }
        [Fact]
        public void ImageGetSet()
        {
            Product p = new Product();
            p.Image = "https://image-url.com";
            Assert.True(p.Image == "https://image-url.com");
        }
        [Fact]
        public async Task CreateAsync()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("CreateProduct").Options;

            using(ReFreshDbContext context = new ReFreshDbContext(options))
            {
                Product testProduct = new Product();
                testProduct.Sku = 1;
                testProduct.Name = "Test Product";
                testProduct.Price = 5;
                testProduct.Description = "This is a test";
                testProduct.Image = "https://image-url.com";

                InventoryManagementService ims = new InventoryManagementService(context);
                await ims.CreateAsync(testProduct);
                var result = context.Inventory.FirstOrDefaultAsync(pr => pr.ID == testProduct.ID);

                Assert.True(testProduct.Sku == result.Result.Sku);
            }
        }
        [Fact]
        public async Task DeleteAsync()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("DeleteProduct").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                Product testProduct = new Product();
                testProduct.Sku = 1;
                testProduct.Name = "Test Product";
                testProduct.Price = 5;
                testProduct.Description = "This is a test";
                testProduct.Image = "https://image-url.com";

                InventoryManagementService ims = new InventoryManagementService(context);
                await ims.CreateAsync(testProduct);
                await ims.DeleteAsync(testProduct.ID);

                await Assert.ThrowsAsync<ArgumentNullException> (()=> ims.DeleteAsync(testProduct.ID));
            }
        }
        [Fact]
        public async Task GetAllAsync()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("GetAllProduct").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                Product testProduct = new Product();
                testProduct.Sku = 1;
                testProduct.Name = "Test Product";
                testProduct.Price = 5;
                testProduct.Description = "This is a test";
                testProduct.Image = "https://image-url.com";

                InventoryManagementService ims = new InventoryManagementService(context);
                await ims.CreateAsync(testProduct);

                List<Product> result = await ims.GetAllAsync() as List<Product>;
                Assert.True(result.Count == 1);
            }
        }
        [Fact]
        public async Task GetOneAsync()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("GetOneProduct").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                Product testProduct = new Product();
                testProduct.Sku = 1;
                testProduct.Name = "Test Product";
                testProduct.Price = 5;
                testProduct.Description = "This is a test";
                testProduct.Image = "https://image-url.com";

                InventoryManagementService ims = new InventoryManagementService(context);
                await ims.CreateAsync(testProduct);

                Product result = ims.GetOneByIdAsync(testProduct.ID).Result;
                Assert.True(result.ID == testProduct.ID);
            }
        }
        [Fact]
        public async Task UpdateAsync()
        {
            DbContextOptions<ReFreshDbContext> options = new DbContextOptionsBuilder<ReFreshDbContext>().UseInMemoryDatabase("UpdateProduct").Options;

            using (ReFreshDbContext context = new ReFreshDbContext(options))
            {
                Product testProduct = new Product();
                testProduct.ID = 1;
                testProduct.Sku = 1;
                testProduct.Name = "Test Product";
                testProduct.Price = 5;
                testProduct.Description = "This is a test";
                testProduct.Image = "https://image-url.com";

                InventoryManagementService ims = new InventoryManagementService(context);
                await ims.CreateAsync(testProduct);

                Product updateProduct = testProduct;
                updateProduct.Name = "Update Product";

                await ims.UpdateAsync(updateProduct);
                Product p = await ims.GetOneByIdAsync(testProduct.ID);

                Assert.True(p.Name == "Update Product");
            }
        }
    }
}
