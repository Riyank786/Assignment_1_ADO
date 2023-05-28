using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authorization;

using Assignment_1_ADO.Models;
using Assignment_1_ADO.Controllers;

namespace Assignment_1_ADO.Repository
{
    public class EmployeeRepository
    {
        private readonly string _connectionString;
        private SqlConnection _connection;
        private readonly UserController _userController;

        public EmployeeRepository(IConfiguration configuration, UserController userController)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
            _userController = userController;
        }

        public Employee GetUser(string userId)
        {
            _connection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = _connection;
            command.CommandText = "SELECT * FROM Employees WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", userId);
            SqlDataReader reader = command.ExecuteReader();
            Employee employee = new Employee();
            while (reader.Read())
            {
                employee.Id = reader.GetInt32(0);
                employee.Name = reader.GetString(1);
                employee.Position = reader.GetString(2);
            }
            _connection.Close();
            return employee;
        }
        public List<Employee> GetAllEmployees()
        {
            _connection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = _connection;
            command.CommandText = "SELECT * FROM Employees";
            SqlDataReader reader = command.ExecuteReader();
            List<Employee> employees = new List<Employee>();
            while (reader.Read())
            {
                Employee employee = new Employee();
                employee.Id = reader.GetInt32(0);
                employee.Name = reader.GetString(1);
                employee.Position = reader.GetString(2);
                employees.Add(employee);
            }
            _connection.Close();
            return employees;
        }

        public Employee GetEmployee(int id)
        {
            _connection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = _connection;
            command.CommandText = "SELECT * FROM Employees WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
            SqlDataReader reader = command.ExecuteReader();
            Employee employee = new Employee();
            while (reader.Read())
            {
                employee.Id = reader.GetInt32(0);
                employee.Name = reader.GetString(1);
                employee.Position = reader.GetString(2);
            }
            _connection.Close();
            return employee;
        }

        public string AddEmployee(Employee employee)
        {
            _connection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = _connection;
            command.CommandText = "INSERT INTO Employees (Name, Position) VALUES (@Name, @Position)";
            command.Parameters.AddWithValue("@Name", employee.Name);
            command.Parameters.AddWithValue("@Position", employee.Position);
            command.ExecuteNonQuery();
            _userController.AddUser(employee.Name);
            _connection.Close();
            return "Employee Added Successfully";
        }

        public string UpdateEmployee(Employee employee)
        {
            _connection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = _connection;
            command.CommandText = "UPDATE Employees SET Name = @Name, Position = @Position WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", employee.Id);
            command.Parameters.AddWithValue("@Name", employee.Name);
            command.Parameters.AddWithValue("@Position", employee.Position);
            command.ExecuteNonQuery();
            _connection.Close();
            return "Employee Updated Successfully";
        }

        public string DeleteEmployee(int id)
        {
            _connection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = _connection;
            command.CommandText = "DELETE FROM Employees WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
            _connection.Close();
            return "Employee Deleted Successfully";
        }
    }
}