using System;

using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;

namespace RecieptManagerAPI.Models
{
    public class Reciept
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [Required]
        [JsonProperty("supplier")]
        public string Supplier { get; set; }
        
        [Required]
        [JsonProperty("amount")]
        public double Amount { get; set; }
        
        [Required]
        [JsonProperty("coin")]
        public string Coin { get; set;}
        
        [Required]
        [JsonProperty("dateOf")]
        public string DateOf { get; set; }
        
        [JsonProperty("userName")]
        public string UserName { get; set; }
        
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}