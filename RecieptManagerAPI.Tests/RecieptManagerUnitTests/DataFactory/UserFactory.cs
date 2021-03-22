using System.Collections;
using System.Collections.Generic;

using RecieptManagerAPI.Models;

using Bogus;

namespace RecieptManagerUnitTests.DataFactory
{
    //Static Class to create random User objects.
    public static class UserFactory
    {
        private static Faker<User> fakerUser = new Faker<User>()
        .RuleFor(o => o.Password, f => f.Internet.Password())
        .RuleFor(o => o.UserName, f => f.Internet.UserName());

        public static User GetUser(){
            return fakerUser.Generate();
        }
    }
}