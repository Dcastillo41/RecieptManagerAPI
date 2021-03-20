using System.Collections;
using System.Collections.Generic;

using RecieptManagerAPI.Models;

using Bogus;

namespace RecieptManagerUnitTests.DataFactory
{
    public static class UserFactory
    {
        private static Faker<User> fakerUser = new Faker<User>()
        .RuleFor(o => o.Password, f => f.Internet.Password())
        .RuleFor(o => o.UserName, f => f.Name.FirstName());

        public static User GetUser(){
            return fakerUser.Generate();
        }
    }
}