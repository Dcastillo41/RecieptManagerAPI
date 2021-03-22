using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using RecieptManagerUnitTests.Models;
using RecieptManagerUnitTests.DataFactory;
using RecieptManagerUnitTests.MockServices;

using RecieptManagerAPI.Models;
using RecieptManagerAPI.Controllers;

using Moq;

namespace RecieptManagerUnitTests
{
    [TestClass, TestCategory("RecieptsUnitTests")]
    public class RecieptsEndpointsTest
    {
        
        [TestMethod]
        public async Task CreateReciept_NonExistingUser_ReturnNotFound()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var reciept = RecieptFactory.GetReciept();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.CreateReciept(reciept.UserName, reciept);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundObjectResult) , "CreateReciept endpoint on a non existing User should return NotFound(404)");
        }

        [TestMethod]
        public async Task CreateReciept_ExistingUserRecieptWithMissingRequiredFields_ReturnBadRequest()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var reciept = database.First().Reciepts.First();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //add a error on the ModelState to mock the fail of a model validation
            userController.ModelState.AddModelError("ValidationError", "Missing reciept requiered fields");

            //Act
            var actionResult = await userController.CreateReciept(reciept.UserName, reciept);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult) , "CreateReciept endpoint on a existing User but reciept with missing requiered fields should return BadRequest(400)");
        }

        [TestMethod]
        public async Task CreateReciept_CorrectReciept_ReturnCreated()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var reciept = database.First().Reciepts.First();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.CreateReciept(reciept.UserName, reciept);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedResult) , "CreateReciept endpoint on a correct reciept should return Created(201)");
        }

        [TestMethod]
        public async Task GetReciept_NonExistingUser_ReturnNotFound()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var reciept = RecieptFactory.GetReciept();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.GetReciept(reciept.UserName, reciept.Id);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundObjectResult) , "GetReciept endpoint on non existing user should return NotFound(404)");
        }

        [TestMethod]
        public async Task GetReciept_NonExistingRecieptId_ReturnNotFound()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var reciept = RecieptFactory.GetReciept();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.GetReciept(database.First().User.UserName, reciept.Id);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundObjectResult) , "GetReciept endpoint on a existing user but non existing RecieptId should return NotFound(404)");
        }

        [TestMethod]
        public async Task GetReciept_ExistingReciept_ReturnOk()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var reciept = database.First().Reciepts.First();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.GetReciept(database.First().User.UserName, reciept.Id);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult) , "GetReciept endpoint on a existing reciept should return Ok(200)");
        }

        [TestMethod]
        public async Task GetReciepts_NonExistingUser_ReturnNotFound()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var reciept = RecieptFactory.GetReciept();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.GetReciepts(reciept.UserName);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundObjectResult) , "GetReciepts endpoint on non existing user should return NotFound(404)");
        }

        [TestMethod]
        public async Task GetReciepts_ExistingUser_ReturnOk()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var reciept = database.First().Reciepts.First();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.GetReciepts(reciept.UserName);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult) , "GetReciepts endpoint on a existing user should return Ok(200)");
        }

        [TestMethod]
        public async Task UpdateReciept_NonExistingUser_ReturnNotFound()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var reciept = RecieptFactory.GetReciept();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.UpdateReciept(reciept.UserName, reciept.Id, reciept);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundObjectResult) , "UpdateReciept endpoint on non existing user should return NotFound(404)");
        }

        [TestMethod]
        public async Task UpdateReciept_NonExistingRecieptId_ReturnNotFound()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var reciept = RecieptFactory.GetReciept();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.UpdateReciept(database.First().User.UserName, reciept.Id, reciept);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundObjectResult) , "UpdateReciept endpoint on a non existing reciept id should return NotFound(404)");
        }

        [TestMethod]
        public async Task UpdateReciept_RecieptWithMissingRequieredFields_ReturnBadRequest()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var reciept = database.First().Reciepts.First();
            reciept.Supplier = null;

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //add a error on the ModelState to mock the fail of a model validation
            userController.ModelState.AddModelError("ValidationError", "Missing reciept requiered fields");

            //Act
            var actionResult = await userController.UpdateReciept(reciept.UserName, reciept.Id, reciept);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult) , "UpdateReciept endpoint on a reciept with missing required fields should return BadRequest(400)");
        }

        [TestMethod]
        public async Task UpdateReciept_CorrectReciept_ReturnOk()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var reciept = database.First().Reciepts.First();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.UpdateReciept(reciept.UserName, reciept.Id, reciept);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult) , "UpdateReciept endpoint on a correct reciept should return Ok(200)");
        }

        [TestMethod]
        public async Task DeleteReciept_NonExistingUser_ReturnNotFound()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var reciept = RecieptFactory.GetReciept();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.DeleteReciept(reciept.UserName, reciept.Id);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundObjectResult) , "DeleteReciept endpoint on a non existing User should return NotFound(404)");
        }

        [TestMethod]
        public async Task DeleteReciept_NonExistingRecieptId_ReturnNotFound()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var reciept = RecieptFactory.GetReciept();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.DeleteReciept(database.First().User.UserName, reciept.Id);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundObjectResult) , "DeleteReciept endpoint on a non existing RecieptId should return NotFound(404)");
        }

        [TestMethod]
        public async Task DeleteReciept_ExistingReciept_ReturnOk()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var reciept = database.First().Reciepts.First();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.DeleteReciept(reciept.UserName, reciept.Id);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult) , "DeleteReciept endpoint on a existing RecieptId should return Ok(200)");
        }
    }
}
