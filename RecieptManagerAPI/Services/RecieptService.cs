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
    public interface IRecieptService
    {
        Task<IEnumerable<Reciept>> GetRecieptsByUserName(string userName);
        Task<Reciept> UpdateReciept(Reciept reciept);
        Task<bool> CreateReciept(Reciept reciept);
        Task<bool> DeleteReciept(int id);
        Task<Reciept> GetRecieptById(int id);

    }
    public class RecieptService : IRecieptService
    {
        private readonly DatabaseConfig _databaseConfig;
        private readonly ILogger<RecieptService> _logger;
        private readonly SqlConnectionStringBuilder _sqlConnectionBuilder;
        
        public RecieptService(ILogger<RecieptService> logger, DatabaseConfig databaseConfig){
            _logger = logger;
            _databaseConfig = databaseConfig;
            _sqlConnectionBuilder = new SqlConnectionStringBuilder(){
                DataSource = _databaseConfig.ServerName,
                UserID = _databaseConfig.UserName,
                Password = _databaseConfig.Password,
                InitialCatalog = _databaseConfig.DatabaseName
            };
        }

        public async Task<IEnumerable<Reciept>> GetRecieptsByUserName(string userName){
            List<Reciept> recieptsByUserName = new List<Reciept>();
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionBuilder.ConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(GetRecieptsByUserNameQuery(userName), connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                recieptsByUserName.Add(new Reciept(){
                                    Id = reader.GetInt32(0),
                                    Supplier = reader.GetString(1),
                                    Amount = reader.GetDouble(2),
                                    Coin = reader.GetString(3),
                                    DateOf = reader.GetDateTime(4).ToString(),
                                    UserName = reader.GetString(5),
                                    Comment = reader.GetString(6)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                _logger.LogInformation($"SQLException with message: {e.Message} and trace: {e.StackTrace}");
            }
            
            return recieptsByUserName;
        }

        private static string GetRecieptsByUserNameQuery(string userName){
            return @"
                SELECT * FROM Reciepts
                WHERE USERNAME = '{userName}'
            ".Replace("{userName}", userName);
        }

        public async Task<Reciept> UpdateReciept(Reciept reciept){
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionBuilder.ConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(UpdateRecieptQuery(reciept), connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException e)
            {
                _logger.LogInformation($"SQLException with message: {e.Message} and trace: {e.StackTrace}");
                throw e;
            }

            return reciept;
        }

        private static string UpdateRecieptQuery(Reciept reciept){
            return @"
                UPDATE Reciepts 
                SET SUPPLIER = '{supplier}', AMOUNT = {amount}, COIN = '{coin}', DATEOF = '{dateOf}', COMMENT = '{comment}'
                WHERE ID = {id}
            ".Replace("{supplier}", reciept.Supplier)
            .Replace("{amount}", reciept.Amount.ToString())
            .Replace("{coin}", reciept.Coin)
            .Replace("{dateOf}", reciept.DateOf)
            .Replace("{comment}", reciept.Comment)
            .Replace("{id}", reciept.Id.ToString());
        }

        public async Task<bool> CreateReciept(Reciept reciept){
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionBuilder.ConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(CreateRecieptQuery(reciept), connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException e)
            {
                _logger.LogInformation($"SQLException with message: {e.Message} and trace: {e.StackTrace}");
                throw e;
            }

            return true;
        }

        private static string CreateRecieptQuery(Reciept reciept){
            return @"
                INSERT INTO Reciepts VALUES ('{supplier}', {amount}, '{coin}', '{dateOf}', '{userName}', '{comment}' )
            ".Replace("{supplier}", reciept.Supplier)
            .Replace("{amount}", reciept.Amount.ToString())
            .Replace("{coin}", reciept.Coin)
            .Replace("{dateOf}", reciept.DateOf)
            .Replace("{userName}", reciept.UserName)
            .Replace("{comment}", reciept.Comment);
        }
        
        public async Task<bool> DeleteReciept(int id){
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionBuilder.ConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(DeleteRecieptQuery(id), connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException e)
            {
                _logger.LogInformation($"SQLException with message: {e.Message} and trace: {e.StackTrace}");
                throw e;
            }

            return true;
        }

        private static string DeleteRecieptQuery(int id){
            return @"
                DELETE Reciepts
                WHERE ID = '{id}'
            ".Replace("{id}", id.ToString());
        }
        
        public async Task<Reciept> GetRecieptById(int id){
            Reciept recieptByUserName = null;
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionBuilder.ConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(GetRecieptQuery(id), connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                recieptByUserName = new Reciept(){
                                    Id = reader.GetInt32(0),
                                    Supplier = reader.GetString(1),
                                    Amount = reader.GetDouble(2),
                                    Coin = reader.GetString(3),
                                    DateOf = reader.GetDateTime(4).ToString(),
                                    UserName = reader.GetString(5),
                                    Comment = reader.GetString(6)
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
            
            return recieptByUserName;
        }

        private static string GetRecieptQuery(int id){
            return @"
                SELECT * FROM Reciepts
                WHERE ID = '{id}'
            ".Replace("{id}", id.ToString());
        }
    }
}