using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using RecieptManagerAPI.Models;

using RecieptManagerUnitTests.DataFactory;

using Newtonsoft.Json;

namespace RecieptManagerE2ETests
{
    [TestClass, TestCategory("UserE2ETests")]
    public class UserEndpointsE2ETests: E2ETestBaseClass
    {
        [TestMethod]
        public async Task CreateUser_UserWithNoPassword_ReturnBadRequest(){
            //Arrange
            var user = UserFactory.GetUser();
            user.Password = null;

            //Act
            var response = await _APIService.CreateUser(user);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode, "CreateUser on a user with no Password should return BadRequest(400)");
        }

        [TestMethod]
        public async Task CreateUser_UserWithNoUserName_ReturnBadRequest(){
            //Arrange
            var user = UserFactory.GetUser();
            user.UserName = null;

            //Act
            var response = await _APIService.CreateUser(user);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode, "CreateUser on a user with no UserName should return BadRequest(400)");
        }

        [TestMethod]
        public async Task CreateUser_ExistingUser_ReturnBadRequest(){
            //Arrange
            var user = _registeredUser;

            //Act
            var response = await _APIService.CreateUser(user);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode, "CreateUser on a existing user should return BadRequest(400)");
        }

        [TestMethod]
        public async Task GetUser_NonExistingUser_ReturnNotFound(){
            //Arrange
            var user = UserFactory.GetUser();

            //Act
            var response = await _APIService.GetUser(user.UserName);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode, "GetUser on a non existing user should return NotFound(404)");
        }

        [TestMethod]
        public async Task GetUser_ExistingUser_ReturnOk(){
            //Arrange
            var user = _registeredUser;

            //Act
            var response = await _APIService.GetUser(user.UserName);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode, "GetUser on a existing user should return OK(200)");
            
            var strResponse = await response.Content.ReadAsStringAsync();
            var responseUser = JsonConvert.DeserializeObject<User>(strResponse);

            //Assert
            Assert.AreEqual(user.UserName, responseUser.UserName, "GetUser on a existing User should return a User with the same UserName");
        }

        [TestMethod]
        public async Task UpdateUser_NonExistingUser_ReturnNotFound(){
            //Arrange
            var user = UserFactory.GetUser();

            //Act
            var response = await _APIService.UpdateUser(user.UserName, user);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode, "UpdateUser on a non existing user should return NotFound(404)");
        }

        [TestMethod]
        public async Task UpdateUser_UserWithNoPassword_ReturnBadRequest(){
            //Arrange
            var user = _registeredUser;
            user.Password = null;

            //Act
            var response = await _APIService.UpdateUser(user.UserName, user);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode, "UpdateUser on a user with no Password should return BadRequest(400)");
        }

        [TestMethod]
        public async Task UpdateUser_ExistingUserWithDifferentUserName_ReturnBadRequest(){
            //Arrange
            var user = UserFactory.GetUser();

            //Act
            var response = await _APIService.UpdateUser(_registeredUser.UserName, user);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode, "UpdateUser on a existing user but with different UserName should return BadRequest(400)");
        }

        [TestMethod]
        public async Task UpdateUser_CorrectUser_ReturnOk(){
            //Arrange
            var user = _registeredUser;

            //Act
            var response = await _APIService.UpdateUser(user.UserName, user);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode, "UpdateUser on a existing user should return OK(200)");
            
            var strResponse = await response.Content.ReadAsStringAsync();
            var responseUser = JsonConvert.DeserializeObject<User>(strResponse);

            //Assert
            Assert.AreEqual(user.UserName, responseUser.UserName, "UpdateUser on a existing User should return a User with the same UserName");
        }

        [TestMethod]
        public async Task DeleteUser_NonExistingUser_ReturnNotFound(){
            //Arrange
            var user = UserFactory.GetUser();

            //Act
            var response = await _APIService.DeleteUser(user.UserName);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode, "DeleteUser on a non existing user should return NotFound(404)");
        }

        [TestMethod]
        public async Task DeleteUser_CorrectUser_ReturnOk(){
            //Arrange
            var user = UserFactory.GetUser();

            //Act
            var createUserResponse = await _APIService.CreateUser(user);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, createUserResponse.StatusCode, "CreateUser on a non existing user should return Created(201)");
            
            var response = await _APIService.DeleteUser(user.UserName);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode, "DeleteUser on a existing user should return OK(200)");

            var strResponse = await response.Content.ReadAsStringAsync();
            bool deleted = JsonConvert.DeserializeObject<bool>(strResponse);
            
            //Assert
            Assert.AreEqual(true, deleted, "DeleteUser on a existing user should return true");
        }
    }
}
