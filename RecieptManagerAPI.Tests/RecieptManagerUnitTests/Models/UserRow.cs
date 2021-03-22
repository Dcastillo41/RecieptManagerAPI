using System.Collections.Generic;
using RecieptManagerAPI.Models;

namespace RecieptManagerUnitTests.Models
{
    public class UserRow
    {
        public User User { get; set; }
        public List<Reciept> Reciepts { get; set; }        
    }
}