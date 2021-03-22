namespace RecieptManagerE2ETests.Config
{
    public class RecieptManagerAPIE2ETestConfig
    {
        public static string Name = "RecieptManagerAPIE2ETestConfig";
        public string BaseUrl { get; set; }
        public string CreateUser { get; set; }
        public string GetUser { get; set; }
        public string UpdateUser { get; set; }
        public string DeleteUser { get; set; }
        public string CreateReciept { get; set; }
        public string GetReciept { get; set; }
        public string GetReciepts { get; set; }
        public string UpdateReciept { get; set; }
        public string DeleteReciept { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RecieptId { get; set; }
    }
}