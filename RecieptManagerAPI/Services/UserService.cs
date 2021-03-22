using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using RecieptManagerAPI.Config;
using RecieptManagerAPI.Models;

namespace RecieptManagerAPI.Services
{
    public interface IUserService
    {
        Task<User> UpdateUser(User user);
        Task<User> CreateUser(User user);
        Task<User> GetUserByUserName(string userName);
        Task<bool> DeleteUserByUserName(string userName);
    }

    public class UserService: IUserService
    {
        private readonly DatabaseConfig _databaseConfig;
        private readonly ILogger<UserService> _logger;
        private readonly SqlConnectionStringBuilder _sqlConnectionBuilder;
        
        public UserService(ILogger<UserService> logger, DatabaseConfig databaseConfig){
            _logger = logger;
            _databaseConfig = databaseConfig;
            _sqlConnectionBuilder = new SqlConnectionStringBuilder(){
                DataSource = _databaseConfig.ServerName,
                UserID = _databaseConfig.UserName,
                Password = _databaseConfig.Password,
                InitialCatalog = _databaseConfig.DatabaseName
            };
        }
        public async Task<User> UpdateUser(User user){
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionBuilder.ConnectionString))
                {
                    connection.Open();
                    string request = UpdateUserQuery(user);
                    _logger.LogInformation($"Update User SQL request: {request}");

                    using (var command = new SqlCommand(request, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException e)
            {
                _logger.LogInformation($"SQLException with message: {e.Message} and trace: {e.StackTrace}");
                return null;
            }

            return user; 
        }

        private static string UpdateUserQuery(User user){
            return @"
                UPDATE Users
                SET PASSWORD = '{password}'
                WHERE USERNAME = '{userName}'
            ".Replace("{userName}", user.UserName)
            .Replace("{password}", user.Password);
        } 
        
        public async Task<User> CreateUser(User user){
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionBuilder.ConnectionString))
                {
                    connection.Open();
                    string request = CreateUserQuery(user);
                    _logger.LogInformation($"CreateUser SQL request: {request}");

                    using (var command = new SqlCommand(request, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException e)
            {
                _logger.LogInformation($"SQLException with message: {e.Message} and trace: {e.StackTrace}");
                return null;
            }

            return user; 
        }

        private static string CreateUserQuery(User user){
            return @"
                INSERT INTO Users VALUES ('{userName}', '{password}')
            ".Replace("{userName}", user.UserName)
            .Replace("{password}", user.Password);
        } 
        
        public async Task<User> GetUserByUserName(string userName){
            User userByUserName = null;
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionBuilder.ConnectionString))
                {
                    connection.Open();
                    string request = GetUserByUserNameQuery(userName);
                    _logger.LogInformation($"GetUserByName SQL request: {request}");

                    using (var command = new SqlCommand(request, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                userByUserName = new User(){
                                    UserName = reader.GetString(0),
                                    Password = reader.GetString(1)
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                _logger.LogInformation($"SQLException with message: {e.Message} and trace: {e.StackTrace}");
            }
            
            return userByUserName;
        }

        private static string GetUserByUserNameQuery(string userName){
            return @"
                SELECT * FROM Users
                WHERE USERNAME = '{userName}'
            ".Replace("{userName}", userName);
        } 

        public async Task<bool> DeleteUserByUserName(string userName){
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionBuilder.ConnectionString))
                {
                    connection.Open();
                    string request = DeleteUserByUserNameQuery(userName);
                    _logger.LogInformation($"DeleteUser SQL request: {request}");

                    using (var command = new SqlCommand(request, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException e)
            {
                _logger.LogInformation($"SQLException with message: {e.Message} and trace: {e.StackTrace}");
                return false;
            }

            return true; 
        }

        private static string DeleteUserByUserNameQuery(string userName){
            return @"
                DELETE FROM Users
                WHERE USERNAME = '{userName}'
            ".Replace("{userName}", userName);
        } 
    }
}