using System;
using System.Data.SqlClient;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Assignment_1_ADO.Models;
using Assignment_1_ADO.Services;


namespace Assignment_1_ADO.Repository
{
    public class UserRepository
    {
        private readonly string _connectionString;
        private readonly PasswordService _passwordService;
        private readonly JwtService _jwtService;
        private SqlConnection _connection;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _passwordService = new PasswordService();
            _jwtService = new JwtService(configuration);
            _connection = new SqlConnection(_connectionString);
        }

        public void AddUser(string userName)
        {
            _connection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = _connection;
            command.CommandText = "INSERT INTO Users (Username, Password, Role) VALUES (@UserName, @Password, @Role)";
            command.Parameters.AddWithValue("@UserName", userName);
            command.Parameters.AddWithValue("@Password", _passwordService.HashPassword("123456"));
            command.Parameters.AddWithValue("@Role", "User");
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public void DeleteUser(string userName)
        {
            _connection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = _connection;
            command.CommandText = "DELETE FROM Users WHERE Username = @UserName";
            command.Parameters.AddWithValue("@UserName", userName);
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public string ChangePassword(string userName, string password)
        {
            _connection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = _connection;
            command.CommandText = "UPDATE Users SET Password = @Password WHERE Username = @UserName";
            command.Parameters.AddWithValue("@UserName", userName);
            command.Parameters.AddWithValue("@Password", _passwordService.HashPassword(password));
            command.ExecuteNonQuery();
            _connection.Close();
            return "Password changed";
        }

        public IActionResult Login(User user)
        {
            _connection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = _connection;
            command.CommandText = "SELECT * FROM Users WHERE Username = @UserName";
            command.Parameters.AddWithValue("@UserName", user.Username);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                if (_passwordService.VerifyPassword(user.Password, reader.GetString(2)))
                {
                    
                    int userId = reader.GetInt32(0);
                    string token = _jwtService.GenerateToken(reader.GetString(1), reader.GetString(3), userId.ToString());
                    var response =  new {
                        token = token,
                        user = new {
                            id = userId,
                            username = reader.GetString(1),
                            role = reader.GetString(3)
                        }
                    };
                    return new OkObjectResult(response);
                }
            }
            _connection.Close();
            return new BadRequestResult();
        }
    }
}
