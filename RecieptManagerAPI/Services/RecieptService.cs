using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using RecieptManagerAPI.Models;

namespace RecieptManagerAPI.Services
{
    public interface IRecieptService
    {
        Task<IEnumerable<Reciept>> GetRecieptsByUserName(string userName);
        Task<Reciept> UpdateReciept(Reciept reciept);
        Task<Reciept> CreateReciept(Reciept reciept);
        Task<bool> DeleteReciept(Reciept reciept);
        Task<Reciept> GetRecieptById(int id);

    }
    public class RecieptService : IRecieptService
    {
        public async Task<IEnumerable<Reciept>> GetRecieptsByUserName(string userName){
            return new List<Reciept>();
        }

        public async Task<Reciept> UpdateReciept(Reciept reciept){
            return new Reciept();
        }

        public async Task<Reciept> CreateReciept(Reciept reciept){
            return new Reciept();
        }
        
        public async Task<bool> DeleteReciept(Reciept reciept){
            return true;
        }
        
        public async Task<Reciept> GetRecieptById(int id){
            return new Reciept();
        }
    }
}