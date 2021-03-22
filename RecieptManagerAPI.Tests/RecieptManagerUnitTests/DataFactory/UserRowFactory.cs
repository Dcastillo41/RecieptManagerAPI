using System.Collections;
using System.Collections.Generic;

using RecieptManagerAPI.Models;

using RecieptManagerUnitTests.Models;

using Bogus;

namespace RecieptManagerUnitTests.DataFactory
{
    //Static Class to generate random UserRow objects.
    public static class UserRowFactory
    {
        //Generates a UserRow with a Reiciepts list of size {recieptsSize}
        public static UserRow GetUserRow(int recieptsSize =2){
            var user = UserFactory.GetUser();
            return new UserRow(){
                User = user,
                Reciepts = RecieptFactory.GetRecieptList(user, recieptsSize)
            };
        }

        public static List<UserRow> GetDatabase(int databaseSize = 1){
            var database = new List<UserRow>();
            for(int i=0; i< databaseSize; ++i){
                database.Add(GetUserRow());
            }
            return database;
        }
    }
}