using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Product_To_Cart()
        {
            //Arrange
            Cart myCart = new Cart();

            Product myProd = new Product
            {
                Name = "P1",
                Category = "Cat1",
                Description = "D1",
                Price = 10,
                ProductID = 1
            };

            Product myProd2 = new Product
            {
                Name = "P2",
                Category = "Cat2",
                Description = "D2",
                Price = 10,
                ProductID = 2
            };

            //Act
            myCart.AddItem(myProd, 1);
            myCart.AddItem(myProd2, 1);
            CartLine[] result = myCart.Lines.ToArray();
            //Assert
            Assert.Equal(2, result.Length);
            Assert.Equal(myProd, result[0].Product);
            Assert.Equal(myProd2, result[1].Product);
        }

        [Fact]
        public void Can_Increment_Quantity_Of_Existing_Product()
        {
            //Arrange
            Cart myCart = new Cart();
            Product p1 = new Product { ProductID = 1, Price = 10, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Price = 15, Name = "P2" };

            //Act
            myCart.AddItem(p1, 1);
            myCart.AddItem(p2, 1);

            myCart.AddItem(p1, 5);

            CartLine[] result = myCart.Lines.ToArray();

            //Assert
            Assert.Equal(6, result[0].Quantity);
            Assert.Equal(1, result[1].Quantity);
            Assert.Equal(2, result.Length);
        }

        [Fact]
        public void Can_Remove_Item_From_Cart()
        {
            //Arrange
            Cart myCart = new Cart();
            Product p1 = new Product { ProductID = 1, Price = 10, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Price = 15, Name = "P2" };

            //Act
            myCart.AddItem(p1, 2);
            myCart.AddItem(p2, 1);

            myCart.RemoveLine(p1);

            CartLine[] result = myCart.Lines.ToArray();

            //Assert
            Assert.Equal(1, result.Length);
            Assert.Equal(p2, result[0].Product);
        }

        [Fact]
        public void Can_Calculate_Total_Price()
        {
            //Arrange
            Cart myCart = new Cart();
            Product p1 = new Product { ProductID = 1, Price = 10, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Price = 15, Name = "P2" };

            //Act
            myCart.AddItem(p1, 2);
            myCart.AddItem(p2, 1);
            decimal result = myCart.ComputeTotalValue();

            //Assert
            Assert.Equal(result, 35);
        }

        [Fact]
        public void Can_Clear_Cart()
        {
            //Arrange
            Cart myCart = new Cart();
            Product p1 = new Product { ProductID = 1, Price = 10, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Price = 15, Name = "P2" };

            //Act
            myCart.AddItem(p1, 2);
            myCart.AddItem(p2, 1);

            myCart.Clear();

            //Assert
            Assert.Equal(0, myCart.Lines.Count());
        }
    }
}
