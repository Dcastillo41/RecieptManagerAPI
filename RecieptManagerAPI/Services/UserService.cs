using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using RecieptManagerAPI.Models;

namespace RecieptManagerAPI.Services
{
    public interface IUserService
    {
        Task<User> UpdateUser(User user);
        Task<User> CreateUser(User user);
        Task<User> GetUserByUserName(string userName);
        Task<bool> DeleteUser(User user);
    }

    public class UserService: IUserService
    {
        private readonly ILogger<UserService> _logger;
        
        public UserService(ILogger<UserService> logger){
            _logger = logger;
        }
        public async Task<User> UpdateUser(User user){
            return new User();
        }
        
        public async Task<User> CreateUser(User user){
            return new User();
        }
        
        public async Task<User> GetUserByUserName(string userName){
            return new User();
        }

        public async Task<bool> DeleteUser(User user){
            return true;
        }
    }
}