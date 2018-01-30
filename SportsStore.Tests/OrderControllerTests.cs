using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
    class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            //checking to see if an empty cart will not allow a user to click checkout. we create an empty cart and order, call the checkout method in the order controller
            //and finally check to see if the SaveOrder method was called, check to make sure a new View is not being returned, and check to see if the model state is properly invalid
            //check OrderController.Checkout() for details

            //Arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            Order order = new Order();
            OrderController target = new OrderController(mock.Object, cart);

            //Act
            ViewResult result = target.Checkout(order) as ViewResult;

            //Assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            //This makes sure that an order with a non-empty cart still doesn't allow a user to checkout if their shipping details have not been filled out
            //it checks the same 3 as above, but this time we add an item to the cart (making it valid) but add an error to the model state (making ModelState.IsValid false)
            //thus it should not call saveorder

            //Arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            OrderController target = new OrderController(mock.Object, cart);
            target.ModelState.AddModelError("error", "error");

            //Act
            ViewResult result = target.Checkout(new Order()) as ViewResult;

            //Assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            //This makes sure that an order with a non-empty cart still doesn't allow a user to checkout if their shipping details have not been filled out
            //it checks the same 3 as above, but this time we add an item to the cart (making it valid) but add an error to the model state (making ModelState.IsValid false)
            //thus it should not call saveorder

            //Arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            OrderController target = new OrderController(mock.Object, cart);

            //Act
            RedirectToActionResult result = target.Checkout(new Order()) as RedirectToActionResult;

            //Assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("Completed", result.ActionName);
        }
    }
}
