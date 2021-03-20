using System;

using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;

namespace RecieptManagerAPI.Models
{
    public class User
    {
        [Required]
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
        
    }
}