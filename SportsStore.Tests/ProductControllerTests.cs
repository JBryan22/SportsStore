﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using SportsStore.Models;
using System.Linq;
using SportsStore.Controllers;
using SportsStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }).AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Act
            ProductsListViewModel model = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            //Assert
            Product[] prodArray = model.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }

        [Fact]
        public void Can_Send_Paging_Info()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }).AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Act
            ProductsListViewModel model = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            //Assert
            var pagingInfo = model.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            //Arrange
            // - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductID = 1, Name="P1", Category="Cat1"},
                new Product {ProductID = 2, Name="P2", Category="Cat2"},
                new Product {ProductID = 3, Name="P3", Category="Cat3"},
                new Product {ProductID = 4, Name="P4", Category="Cat2"}
            }).AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Action
            Product[] result = (controller.List("Cat2", 1).ViewData.Model as ProductsListViewModel).Products.ToArray();

            //Assert

            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [Fact]
        public void Can_Generate_Correct_Page_Count_Based_On_Category()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductID = 1, Name="P1", Category="Cat1"},
                new Product {ProductID = 2, Name="P2", Category="Cat2"},
                new Product {ProductID = 3, Name="P3", Category="Cat3"},
                new Product {ProductID = 4, Name="P4", Category="Cat2"},
                new Product {ProductID = 4, Name="P5", Category="Cat1"}
            }).AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            Func<ViewResult, ProductsListViewModel> GetModel = result => result?.ViewData?.Model as ProductsListViewModel;

            //Act
            int? result1 = GetModel(controller.List("Cat1"))?.PagingInfo.TotalItems;
            int? result2 = GetModel(controller.List("Cat2"))?.PagingInfo.TotalItems;
            int? result3 = GetModel(controller.List("Cat3"))?.PagingInfo.TotalItems;
            int? result4 = GetModel(controller.List(null))?.PagingInfo.TotalItems;

            //Assert
            Assert.Equal(2, result1);
            Assert.Equal(2, result2);
            Assert.Equal(1, result3);
            Assert.Equal(5, result4);
        }
    }
}
