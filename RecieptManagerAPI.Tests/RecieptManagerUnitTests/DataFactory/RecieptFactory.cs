using System.Collections;
using System.Collections.Generic;
using System;

using RecieptManagerAPI.Models;

using Bogus;

namespace RecieptManagerUnitTests.DataFactory
{
    //Static Class to create random Reciept objects.
    public static class RecieptFactory
    {
        private static int _recieptId = 10500;
        private static Faker<Reciept> fakerReciept = new Faker<Reciept>()
        .RuleFor(o => o.Amount, f => f.Random.Double(10.0, 100.0))
        .RuleFor(o => o.Coin, f => f.Random.Word())
        .RuleFor(o => o.Comment, f => f.Random.Words())
        .RuleFor(o => o.DateOf, f => DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"))
        .RuleFor(o => o.Id, f => ++_recieptId)
        .RuleFor(o => o.Supplier, f => f.Company.CompanyName())
        .RuleFor(o => o.UserName, f => f.Internet.UserName());
        
        //Generates a reciept using a given user as a seed to have the same userName.
        public static Reciept GetReciept(User user = null){
            user = (user == null)? UserFactory.GetUser() : user;
            var reciept = fakerReciept.Generate();
            reciept.UserName = user.UserName;
            return reciept;
        }

        //Generates a list of Reciepts of a given size using a user as a seed to have the same userName
        public static List<Reciept> GetRecieptList(User user = null, int size = 2){
            user = (user == null)? UserFactory.GetUser() : user;
            List<Reciept> resultList = new List<Reciept>();
            for(int i =0; i< size; i++){
                resultList.Add(GetReciept(user));
            }
            return resultList;
        }
    }
}