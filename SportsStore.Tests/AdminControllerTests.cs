using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
    public class AdminControllerTests
    {
        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }

        [Fact]
        public void Index_Contains_All_Products()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Name = "P1", ProductID = 1},
                new Product {Name = "P2", ProductID = 2},
                new Product {Name = "P3", ProductID = 3},
                new Product {Name = "P4", ProductID = 4}
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            //Act
            Product[] result = GetViewModel<IEnumerable<Product>>(target.Index())?.ToArray();

            //Assert
            Assert.Equal(4, result.Count());
            Assert.True(result[0].Name == "P1");
            Assert.True(result[1].Name == "P2");
            Assert.True(result[2].Name == "P3");
            Assert.True(result[3].Name == "P4");
        }

        [Fact]
        public void Edit_Returns_Specific_Product()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Name = "P1", ProductID = 1},
                new Product {Name = "P2", ProductID = 2},
                new Product {Name = "P3", ProductID = 3},
                new Product {Name = "P4", ProductID = 4}
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            //Act
            Product result = GetViewModel<Product>(target.Edit(2));

            //Assert
            Assert.True(result.Name == "P2");
        }

        [Fact]
        public void Edit_Returns_Nothing_If_ID_Isnt_Found()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Name = "P1", ProductID = 1},
                new Product {Name = "P2", ProductID = 2},
                new Product {Name = "P3", ProductID = 3},
                new Product {Name = "P4", ProductID = 4}
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            //Act
            Product result = GetViewModel<Product>(target.Edit(6));

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void Edit_Saves_Valid_Changes()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            AdminController target = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };

            Product product = new Product { Name = "Test" };

            //Act
            IActionResult result = target.Edit(product);

            //Assert
            mock.Verify(m => m.SaveProduct(product));
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
        }

        [Fact]
        public void Edit_Doesnt_Save_Indalid_Changes()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            AdminController target = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };
            target.ModelState.AddModelError("error", "error");

            Product product = new Product { Name = "Test" };

            //Act
            IActionResult result = target.Edit(product);

            //Assert
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Valid_Products()
        {
            //Arrange
            Product prod = new Product { ProductID = 2, Name = "Test" };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 3, Name = "P3"}
            }.AsQueryable<Product>());

            AdminController target = new AdminController(mock.Object);

            //Act
            target.Delete(prod.ProductID);

            //Assert
            mock.Verify(m => m.DeleteProduct(prod.ProductID));
        }
    }
}
