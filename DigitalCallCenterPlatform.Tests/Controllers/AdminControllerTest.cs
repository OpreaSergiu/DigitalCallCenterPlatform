using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalCallCenterPlatform;
using DigitalCallCenterPlatform.Controllers;
using DigitalCallCenterPlatform.Models;

namespace DigitalCallCenterPlatform.Tests.Controllers
{
    [TestClass]
    public class AdminControllerTest
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [TestMethod]
        public void Index()
        {
            // Arrange
            AdminController controller = new AdminController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Roles()
        {
            // Arrange
            AdminController controller = new AdminController();

            // Act 
            ViewResult result = controller.Roles() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddAccount()
        {
            // Arrange
            AdminController controller = new AdminController();

            // Act
            ViewResult result = controller.AddAccount() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
