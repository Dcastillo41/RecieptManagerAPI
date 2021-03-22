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
    [TestClass, TestCategory("RecieptE2ETests")]
    public class RecieptEndpointsE2ETests: E2ETestBaseClass
    {
        [TestMethod]
        public async Task CreateReciept_NoExistingUserName_ReturnNotFound(){
            //Arrange
            var reciept = RecieptFactory.GetReciept();

            //Act
            var response = await _APIService.CreateReciept(reciept.UserName, reciept);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode, "CreateReciept on a non existing userName should return NotFound(404)");
        }

        [TestMethod]
        public async Task CreateReciept_RecieptWithMissingRequiredFields_ReturnBadRequest(){
            //Arrange
            var reciept = RecieptFactory.GetReciept(_registeredUser);
            reciept.Supplier = null;

            //Act
            var response = await _APIService.CreateReciept(reciept.UserName, reciept);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode, "CreateReciept on a reciept with missing required fields should return BadRequest(400)");
        }

        [TestMethod]
        public async Task CreateReciept_RecieptWithDifferentUserName_ReturnBadRequest(){
            //Arrange
            var reciept = RecieptFactory.GetReciept();

            //Act
            var response = await _APIService.CreateReciept(_registeredUser.UserName, reciept);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode, "CreateReciept on a reciept with different userName should return BadRequest(400)");
        }

        [TestMethod]
        public async Task GetReciept_NonExistingUserName_ReturnNotFound(){
            //Arrange
            var reciept = RecieptFactory.GetReciept();

            //Act
            var response = await _APIService.GetReciept(reciept.UserName, reciept.Id);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode, "GetReciept on a non existing userName should return NotFound(404)");
        }

        [TestMethod]
        public async Task GetReciept_NonExistingRecieptId_ReturnNotFound(){
            //Arrange
            var reciept = RecieptFactory.GetReciept(_registeredUser);

            //Act
            var response = await _APIService.GetReciept(reciept.UserName, reciept.Id);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode, "GetReciept on a non existing reciept id should return NotFound(404)");
        }

        [TestMethod]
        public async Task GetReciept_ExistingRecieptId_ReturnOk(){
            //Arrange
            var reciept = RecieptFactory.GetReciept(_registeredUser);

            //Act
            var response = await _APIService.GetReciept(reciept.UserName, _testConfig.RecieptId);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode, "GetReciept on a existing reciept id should return OK(200)");

            var strResponse = await response.Content.ReadAsStringAsync();
            var responseReciept = JsonConvert.DeserializeObject<Reciept>(strResponse);
            
            //Assert
            Assert.AreEqual(_testConfig.RecieptId, responseReciept.Id, "GetReciept on a existing reciept id should return a Reciept with the same Id");
        }

        [TestMethod]
        public async Task GetReciepts_NonExistingUserName_ReturnNotFound(){
            //Arrange
            var reciept = RecieptFactory.GetReciept();

            //Act
            var response = await _APIService.GetReciepts(reciept.UserName);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode, "GetReciepts on a non existing userName should return NotFound(404)");
        }

        [TestMethod]
        public async Task GetReciepts_ExistingUserName_ReturnOk(){
            //Arrange
            var reciept = RecieptFactory.GetReciept(_registeredUser);

            //Act
            var response = await _APIService.GetReciepts(reciept.UserName);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode, "GetReciepts on a existing userName should return OK(200)");

            var strResponse = await response.Content.ReadAsStringAsync();
            var responseReciepts = JsonConvert.DeserializeObject<List<Reciept>>(strResponse);
            
            //Assert
            Assert.IsTrue(responseReciepts != null && responseReciepts.Count > 0, "GetReciepts on a existing userName should return a List of Reciepts");
        }

        [TestMethod]
        public async Task UpdateReciept_NonExistingUserName_ReturnNotFound(){
            //Arrange
            var reciept = RecieptFactory.GetReciept();

            //Act
            var response = await _APIService.UpdateReciept(reciept.UserName, reciept.Id, reciept);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode, "UpdateReciept on a non existing userName should return NotFound(404)");
        }

        [TestMethod]
        public async Task UpdateReciept_NonExistingRecieptId_ReturnNotFound(){
            //Arrange
            var reciept = RecieptFactory.GetReciept(_registeredUser);

            //Act
            var response = await _APIService.UpdateReciept(reciept.UserName, reciept.Id, reciept);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode, "UpdateReciept on a non existing recieptId should return NotFound(404)");
        }

        [TestMethod]
        public async Task UpdateReciept_RecieptWithMissingRequiredFields_ReturnBadRequest(){
            //Arrange
            var reciept = RecieptFactory.GetReciept(_registeredUser);
            reciept.Id = _testConfig.RecieptId;
            reciept.Supplier = null;

            //Act
            var response = await _APIService.UpdateReciept(reciept.UserName, reciept.Id, reciept);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode, "UpdateReciept on a reciept with missing required fields should return BadRequest(400)");
        }

        [TestMethod]
        public async Task UpdateReciept_RecieptWithDifferentUserName_ReturnBadRequest(){
            //Arrange
            var reciept = RecieptFactory.GetReciept();
            reciept.Id = _testConfig.RecieptId;

            //Act
            var response = await _APIService.UpdateReciept(_registeredUser.UserName, reciept.Id, reciept);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode, "UpdateReciept on a reciept with different userName should return BadRequest(400)");
        }

        [TestMethod]
        public async Task UpdateReciept_RecieptWithDifferentRecieptId_ReturnBadRequest(){
            //Arrange
            var reciept = RecieptFactory.GetReciept(_registeredUser);

            //Act
            var response = await _APIService.UpdateReciept(_registeredUser.UserName, _testConfig.RecieptId, reciept);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode, "UpdateReciept on a reciept with different recieptId should return BadRequest(400)");
        }

        [TestMethod]
        public async Task UpdateReciept_CorrectReciept_ReturnOk(){
            //Arrange
            var getRecieptResponse = await _APIService.GetReciept(_registeredUser.UserName, _testConfig.RecieptId);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, getRecieptResponse.StatusCode, "GetReciept on a correct reciept should return Ok(200)");
            
            var getRecieptstrResponse = await getRecieptResponse.Content.ReadAsStringAsync();
            var reciept = JsonConvert.DeserializeObject<Reciept>(getRecieptstrResponse);
            
            //Act
            var response = await _APIService.UpdateReciept(reciept.UserName, reciept.Id, reciept);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode, "UpdateReciept on a correct reciept should return Ok(200)");

            var strResponse = await response.Content.ReadAsStringAsync();
            var responseReciept = JsonConvert.DeserializeObject<Reciept>(strResponse);
            
            //Assert
            Assert.AreEqual(reciept.Id, responseReciept.Id, "UpdateReciept on a correct reciept should return a Reciept with the same Id");
        }

        [TestMethod]
        public async Task DeleteReciept_NonExistingUserName_ReturnNotFound(){
            //Arrange
            var reciept = RecieptFactory.GetReciept();

            //Act
            var response = await _APIService.DeleteReciept(reciept.UserName, reciept.Id);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode, "DeleteReciept on a non existing userName should return NotFound(404)");
        }

        [TestMethod]
        public async Task DeleteReciept_NonExistingRecieptId_ReturnNotFound(){
            //Arrange
            var reciept = RecieptFactory.GetReciept(_registeredUser);

            //Act
            var response = await _APIService.DeleteReciept(reciept.UserName, reciept.Id);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode, "DeleteReciept on a non existing recieptId should return NotFound(404)");
        }

        [TestMethod]
        public async Task DeleteReciept_NewCreatedReciept_ReturnOk(){
            var reciept = RecieptFactory.GetReciept(_registeredUser);
            
            //Step 1- Create the reciept that well be erased.
            var createResponse = await _APIService.CreateReciept(reciept.UserName, reciept);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, createResponse.StatusCode, "CreateReciept on a correct recieptId should return Ok(200)");
            
            var createStrResponse = await createResponse.Content.ReadAsStringAsync();
            bool created = JsonConvert.DeserializeObject<bool>(createStrResponse);
            Assert.IsTrue(created, "CreateReciept on a correct reciept should return true");

            //Step 2- Get all the reciepts to get the lastOne, wich was the one created on the step 1.
            var recieptsResponse = await _APIService.GetReciepts(reciept.UserName);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, createResponse.StatusCode, "GetReciepts on a correct userName should return Ok(200)");
            
            var recieptsStr = await recieptsResponse.Content.ReadAsStringAsync();
            var allReciepts = JsonConvert.DeserializeObject<List<Reciept>>(recieptsStr);
            
            Assert.IsTrue(allReciepts.Count > 0, "After created reciept the GetReciepts endpoint should return a list with a least one reciept");
            
            Reciept createdReciept = new Reciept(){Id= 0};
            foreach(var userReciept in allReciepts){
                if(userReciept.Id >= createdReciept.Id) createdReciept = userReciept;
            }

            //Step 3- Delete the reciept created on the step 1.
            var response = await _APIService.DeleteReciept(createdReciept.UserName, createdReciept.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode, "DeleteReciept on a existing reciept should return Ok(200)");

            var strResponse = await response.Content.ReadAsStringAsync();
            bool deleted = JsonConvert.DeserializeObject<bool>(strResponse);
            
            //Assert
            Assert.AreEqual(true, deleted, "DeleteReciept on a existing reciept should return true");            
        }
    }
}