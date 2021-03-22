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
    //Class to mock the IRecieptService interface.
    public class MockRecieptService: IRecieptService
    {
        private readonly List<UserRow> _database;
        
        public MockRecieptService(List<UserRow> database){
            _database = database;
        }

        public async Task<IEnumerable<Reciept>> GetRecieptsByUserName(string userName){
            return _database.Where((userRow) => userRow.User.UserName == userName).FirstOrDefault().Reciepts;
        }

        public async Task<Reciept> UpdateReciept(Reciept reciept){
            foreach(var UserRow in _database.Where((userRow) => userRow.User.UserName == reciept.UserName)){
                UserRow.Reciepts.ForEach((insideReciept) => {
                    if(insideReciept.Id == reciept.Id) insideReciept = reciept;
                });
            }
            return reciept;
        }
        
        public async Task<bool> CreateReciept(Reciept reciept){
            bool result = false;
            foreach(var UserRow in _database.Where((userRow) => userRow.User.UserName == reciept.UserName)){
                UserRow.Reciepts.Add(reciept);
                result = true;
            }
            return result;
        }

        public async Task<bool> DeleteReciept(int id){
            bool deleted = false;
            foreach(var UserRow in _database){
                deleted |= UserRow.Reciepts.RemoveAll((insideReciept) => insideReciept.Id == id) > 0;
            }
            return deleted;
        }

        public async Task<Reciept> GetRecieptById(int id){
            Reciept result = null;
            foreach(var UserRow in _database){
                result = UserRow.Reciepts.FirstOrDefault((insideReciept) => insideReciept.Id == id);
            }
            return result;
        }

    }
}