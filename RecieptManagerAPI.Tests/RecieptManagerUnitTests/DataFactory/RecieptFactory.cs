using System.Collections;
using System.Collections.Generic;

using RecieptManagerAPI.Models;

using Bogus;

namespace RecieptManagerUnitTests.DataFactory
{
    //Class to create random Reciept objects.
    public static class RecieptFactory
    {
        private static Faker<Reciept> fakerReciept = new Faker<Reciept>()
        .RuleFor(o => o.Amount, f => f.Random.Double(1.0, 100.0))
        .RuleFor(o => o.Coin, f => f.Random.Word())
        .RuleFor(o => o.Comment, f => f.Random.Words())
        .RuleFor(o => o.DateTime, f => f.Date.Recent().ToUniversalTime().ToString())
        .RuleFor(o => o.Id, f => f.Random.Decimal(10, 100))
        .RuleFor(o => o.Supplier, f => f.Company.CompanyName())
        .RuleFor(o => o.UserName, f => f.Name.FirstName());

        public static Reciept GetReciept(){
            return fakerReciept.Generate();
        }
    }
}