using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using RecieptManagerAPI.Services;
using RecieptManagerAPI.Models;

using RecieptManagerUnitTests.Models;

using Moq;
using Newtonsoft.Json;


namespace RecieptManagerUnitTests.MockServices
{
    //Class to mock the IUserService interface
    public class MockUserService: IUserService
    {
        private readonly List<UserRow> _database;
        public MockUserService(List<UserRow> database){
            _database = database;
        }

        public async Task<User> UpdateUser(User user){
            foreach(var UserRow in _database){
                if(UserRow.User.UserName == user.UserName){
                    UserRow.User = user;
                }
            }
            return user;
        }

        public async Task<User> CreateUser(User user){
            _database.Add(new UserRow(){
                User = user,
                Reciepts = new List<Reciept>()
            });
            return user;
        }

        public async Task<User> GetUserByUserName(string userName){
            User result = null;
            foreach(var UserRow in _database){
                result = (UserRow.User.UserName == userName)? UserRow.User : null; 
            }
            return result;
        }

        public async Task<bool> DeleteUserByUserName(string userName){
            return _database.RemoveAll((userRow) => userRow.User.UserName == userName) > 0;
        }
    }
}