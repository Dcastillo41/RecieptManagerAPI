using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using RecieptManagerAPI.Services;
using RecieptManagerAPI.Models;

using Newtonsoft.Json;

namespace RecieptManagerAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IRecieptService _recieptService;

        public UserController(ILogger<UserController> logger, IUserService userService, IRecieptService recieptService){
            _logger = logger;
            _userService = userService;
            _recieptService = recieptService;
        }
        
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> CreateUser(User user){
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var userByUserName = await _userService.GetUserByUserName(user.UserName);
            if(userByUserName != null) return BadRequest($"Already exists a user with userName: {user.UserName}");

            var createdUser = await _userService.CreateUser(user);

            return Created("User created successfully", createdUser);
        }

        [HttpGet("{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUser(string userName){
            var userByUserName = await _userService.GetUserByUserName(userName);
            if(userByUserName == null) return NotFound($"User not found for userName: {userName}");

            return Ok(userByUserName);
        }

        [HttpPatch("{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> UpdateUser(string userName, User user){
            var userByUserName = await _userService.GetUserByUserName(userName);
            if(userByUserName == null) return NotFound($"User not found for userName: {userName}");
            
            if(!ModelState.IsValid || userName != user.UserName) return BadRequest(ModelState);

            var updatedUser = await _userService.UpdateUser(user);

            return Ok(updatedUser);
        }

        [HttpDelete("{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteUser(string userName){
            var userByUserName = await _userService.GetUserByUserName(userName);
            if(userByUserName == null) return NotFound($"User not found for userName: {userName}");

            var deleted = await _userService.DeleteUserByUserName(userName); 
            return Ok(deleted);
        }

        [HttpPut("{userName}/reciepts")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> CreateReciept(string userName, Reciept reciept){
            var userByUserName = await _userService.GetUserByUserName(userName);
            if(userByUserName == null) return NotFound($"User not found for userName: {userName}");

            if(!ModelState.IsValid || userName != reciept.UserName) return BadRequest(ModelState);
            reciept.UserName = userName;
            
            var createdReciept = await _recieptService.CreateReciept(reciept);

            return Created("Reciept created successfully", createdReciept);
        }

        [HttpGet("{userName}/reciepts/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Reciept>> GetReciept(string userName, int id){
            var userByUserName = await _userService.GetUserByUserName(userName);
            if(userByUserName == null) return NotFound($"User not found for userName: {userName}");

            var reciept = await _recieptService.GetRecieptById(id);

            if(reciept == null) return NotFound($"Reciept not found for id: {id}");

            return Ok(reciept);
        }

        [HttpGet("{userName}/reciepts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Reciept>>> GetReciepts(string userName){
            var userByUserName = await _userService.GetUserByUserName(userName);
            if(userByUserName == null) return NotFound($"User not found for userName: {userName}");

            var recieptsByUserName = await _recieptService.GetRecieptsByUserName(userName);

            return Ok(recieptsByUserName);
        }

        [HttpPatch("{userName}/reciepts/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Reciept>> UpdateReciept(string userName, int id, Reciept recieptToUpdate){
            var userByUserName = await _userService.GetUserByUserName(userName);
            if(userByUserName == null) return NotFound($"User not found for userName: {userName}");
            
            var reciept = await _recieptService.GetRecieptById(id);
            if(reciept == null) return NotFound($"Reciept not found for id: {id}");

            if(!ModelState.IsValid || userName != recieptToUpdate.UserName || id != recieptToUpdate.Id) return BadRequest(ModelState);

            recieptToUpdate.UserName = userName;
            var updatedReciept = await _recieptService.UpdateReciept(recieptToUpdate);

            return Ok(updatedReciept);
        }

        [HttpDelete("{userName}/reciepts/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteReciept(string userName, int id){
            var userByUserName = await _userService.GetUserByUserName(userName);
            if(userByUserName == null) return NotFound($"User not found for userName: {userName}");

            var reciept = await _recieptService.GetRecieptById(id);
            if(reciept == null) return NotFound($"Reciept not found for id: {id}");

            var deleted = await _recieptService.DeleteReciept(id); 
            return Ok(deleted);
        }

    }
}
