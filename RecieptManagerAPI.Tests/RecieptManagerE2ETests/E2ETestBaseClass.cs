using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using RecieptManagerAPI.Models;

using RecieptManagerE2ETests.Config;
using RecieptManagerE2ETests.Services;

using Newtonsoft.Json;
using Moq;

namespace RecieptManagerE2ETests
{
    public class E2ETestBaseClass
    {
        public User _registeredUser;
        public RecieptManagerAPIService _APIService;
        public RecieptManagerAPIE2ETestConfig _testConfig; 

        private static IConfiguration Configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables().Build(); 

        //Method to initialize the RecieptManagerApiService using the appsetting.json
        [TestInitialize]
        public virtual void Initialize(){
            _testConfig = Configuration.GetSection(RecieptManagerAPIE2ETestConfig.Name).Get<RecieptManagerAPIE2ETestConfig>();
            _APIService = new RecieptManagerAPIService(_testConfig);
            _registeredUser = new User(){
                UserName = _testConfig.UserName,
                Password = _testConfig.Password,
            };
        }
    }
}