using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using RecieptManagerAPI.Models;

using RecieptManagerE2ETests.Config;

using Newtonsoft.Json;

namespace RecieptManagerE2ETests.Services
{
    public class RecieptManagerAPIService
    {
        private readonly RecieptManagerAPIE2ETestConfig _testConfig;
        private readonly HttpClient httpClient = new HttpClient();
        public const string JsonContentType = "application/json";
        public RecieptManagerAPIService(RecieptManagerAPIE2ETestConfig TestConfig){
            _testConfig = TestConfig;
            httpClient.BaseAddress = new System.Uri(TestConfig.BaseUrl);
        }

        //Method to call the RecieptManagerAPI with a given HTTP request
        protected virtual async Task<HttpResponseMessage> call(HttpRequestMessage request) {
            var response = await httpClient.SendAsync(request);
            
            return response;
        }

        public async Task<System.Net.Http.HttpResponseMessage> CreateUser(User user){
            var request = new HttpRequestMessage(HttpMethod.Put, _testConfig.CreateUser);
            request.Content = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, JsonContentType);

            var response = await call(request);
            return response;
        }

        public async Task<System.Net.Http.HttpResponseMessage> GetUser(string userName){
            var request = new HttpRequestMessage(HttpMethod.Get, _testConfig.GetUser.Replace("{userName}", userName));

            var response = await call(request);
            return response;
        }

        public async Task<System.Net.Http.HttpResponseMessage> UpdateUser(string userName, User user){
            var request = new HttpRequestMessage(HttpMethod.Patch, _testConfig.UpdateUser.Replace("{userName}", userName));
            request.Content = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, JsonContentType);

            var response = await call(request);
            return response;
        }

        public async Task<System.Net.Http.HttpResponseMessage> DeleteUser(string userName){
            var request = new HttpRequestMessage(HttpMethod.Delete, _testConfig.DeleteUser.Replace("{userName}", userName));

            var response = await call(request);
            return response;
        }

        public async Task<System.Net.Http.HttpResponseMessage> CreateReciept(string userName, Reciept reciept){
            var request = new HttpRequestMessage(HttpMethod.Put, _testConfig.CreateReciept.Replace("{userName}", userName));
            request.Content = new StringContent(JsonConvert.SerializeObject(reciept), System.Text.Encoding.UTF8, JsonContentType);

            var response = await call(request);
            return response;
        }

        public async Task<System.Net.Http.HttpResponseMessage> GetReciept (string userName, int id){
            var request = new HttpRequestMessage(HttpMethod.Get, _testConfig.GetReciept.Replace("{userName}", userName).Replace("{id}", id.ToString()));

            var response = await call(request);
            return response;
        }

        public async Task<System.Net.Http.HttpResponseMessage> GetReciepts (string userName){
            var request = new HttpRequestMessage(HttpMethod.Get, _testConfig.GetReciepts.Replace("{userName}", userName));

            var response = await call(request);
            return response;
        }

        public async Task<System.Net.Http.HttpResponseMessage> UpdateReciept (string userName, int id, Reciept reciept){
            var request = new HttpRequestMessage(HttpMethod.Patch, _testConfig.UpdateReciept.Replace("{userName}", userName).Replace("{id}", id.ToString()));
            request.Content = new StringContent(JsonConvert.SerializeObject(reciept), System.Text.Encoding.UTF8, JsonContentType);

            var response = await call(request);
            return response;
        }

        public async Task<System.Net.Http.HttpResponseMessage> DeleteReciept (string userName, int id){
            var request = new HttpRequestMessage(HttpMethod.Delete, _testConfig.DeleteReciept.Replace("{userName}", userName).Replace("{id}", id.ToString()));

            var response = await call(request);
            return response;
        }

    }
}