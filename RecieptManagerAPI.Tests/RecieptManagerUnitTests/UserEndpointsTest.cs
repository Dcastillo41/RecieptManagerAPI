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
    [TestClass, TestCategory("UserUnitTests")]
    public class UserEndpointsTest
    {
        [TestMethod]
        public async Task CreateUser_UserWithNoUserName_ReturnBadRequest()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var user = UserFactory.GetUser();
            user.UserName = null;

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //add a error on the ModelState to mock the fail of a model validation
            userController.ModelState.AddModelError("ValidationError", "Missing UserName field");

            //Act
            var actionResult = await userController.CreateUser(user);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult) , "Create user endpoint on a User with no UserName should return BadRequest(400)");
        }

        [TestMethod]
        public async Task CreateUser_UserWithNoPassword_ReturnBadRequest()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var user = UserFactory.GetUser();
            user.Password = null;

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //add a error on the ModelState to mock the fail of a model validation
            userController.ModelState.AddModelError("ValidationError", "Missing Password field");

            //Act
            var actionResult = await userController.CreateUser(user);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult) , "CreateUser endpoint on a User with no Password should return BadRequest(400)");
        }

        [TestMethod]
        public async Task CreateUser_AlreadyExistingUser_ReturnBadRequest()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.CreateUser(database.First().User);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult) , "CreateUser endpoint on already existing user should return BadRequest(400)");
        }

        [TestMethod]
        public async Task CreateUser_NewUser_ReturnCreated()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var user = UserFactory.GetUser();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.CreateUser(user);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedResult) , "CreateUser endpoint on new user should return Created(201)");
        }

        [TestMethod]
        public async Task GetUser_NonExistingUser_ReturnNotFound()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var user = UserFactory.GetUser();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.GetUser(user.UserName);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundObjectResult) , "GetUser endpoint on a non existing user should return NotFound(404)");
        }

        [TestMethod]
        public async Task GetUser_ExistingUser_ReturnOk()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.GetUser(database.First().User.UserName);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult) , "GetUser endpoint on a existing user should return Ok(200)");
        }

        [TestMethod]
        public async Task UpdateUser_NonExistingUser_ReturnNotFound()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var user = UserFactory.GetUser();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 

            //Act
            var actionResult = await userController.UpdateUser(user.UserName, user);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundObjectResult) , "UpdateUser endpoint on a non existing user should return NotFound(404)");
        }

        [TestMethod]
        public async Task UpdateUser_ExistingUserWithNoPassword_ReturnBadRequest()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var user = new User(){UserName = database.First().User.UserName, Password = null};

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 
            
            //add a error on the ModelState to mock the fail of a model validation
            userController.ModelState.AddModelError("ValidationError", "Missing Password field");

            //Act
            var actionResult = await userController.UpdateUser(user.UserName, user);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult) , "UpdateUser endpoint on a existing user but no Password given should return BadRequest(400)");
        }

        [TestMethod]
        public async Task UpdateUser_CorrectExistingUser_ReturnOk()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var user = database.First().User;

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 
            
            //Act
            var actionResult = await userController.UpdateUser(user.UserName, user);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult) , "UpdateUser endpoint on a correct existing user should return Ok(200)");
        }

        [TestMethod]
        public async Task DeleteUser_NonExistingUser_ReturnNotFound()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var user = UserFactory.GetUser();

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 
            
            //Act
            var actionResult = await userController.DeleteUser(user.UserName);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundObjectResult) , "DeleteUser endpoint on a non existing user should return NotFound(404)");
        }

        [TestMethod]
        public async Task DeleteUser_ExistingUser_ReturnOk()
        {
            //Arrange
            var database = UserRowFactory.GetDatabase(1);
            var user = database.First().User;

            var mockUserService = new MockUserService(database);
            var mockRecieptService = new MockRecieptService(database);
            var userController = new UserController(new Mock<ILogger<UserController>>().Object, mockUserService, mockRecieptService); 
            
            //Act
            var actionResult = await userController.DeleteUser(user.UserName);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult) , "DeleteUser endpoint on a existing user should return Ok(200)");
        }
    }
}
