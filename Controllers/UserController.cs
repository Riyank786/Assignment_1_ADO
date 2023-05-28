using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using Assignment_1_ADO.Repository;
using Assignment_1_ADO.Models;

namespace Assignment_1_ADO.Controllers
{

    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;
        public UserController( UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize(Roles = "Admin")]
        public void AddUser(string UserName)
        {
            try {
                _userRepository.AddUser(UserName);
            } catch(Exception e) {
                Console.WriteLine(e);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public void DeleteUser(string UserName) {
            try {
                _userRepository.DeleteUser(UserName);
            } catch(Exception e) {
                Console.WriteLine(e);
            }
        }

        [HttpPatch]
        [Route("changePassword/{password}")]
        public string ChangePassword(string password) {
            try {
                string userName = User.FindFirst(ClaimTypes.Name).Value;
                return _userRepository.ChangePassword(userName, password);
            } catch(Exception e) {
                Console.WriteLine(e);
                return "Error changing password";
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] User user) {
            try {
                return Ok(_userRepository.Login(user));
            } catch(Exception e) {
                Console.WriteLine(e);
                return BadRequest("Error logging in");
            }
        }
    }
}