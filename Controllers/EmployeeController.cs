using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using Assignment_1_ADO.Models;
using Assignment_1_ADO.Repository;

namespace Assignment_1_ADO.Controllers
{

    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : Controller
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeeController(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        [Route("getEmployee")]
        public IActionResult GetUser()
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                return Ok(_employeeRepository.GetUser(userId));
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("getall")]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_employeeRepository.GetAllEmployees());
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("get/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_employeeRepository.GetEmployee(id));
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("add")]
        public IActionResult Add([FromBody] Employee employee)
        {
            try 
            {
                return Ok(_employeeRepository.AddEmployee(employee));
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("update")]
        public IActionResult Update([FromBody] Employee employee) {
            try 
            {
                return Ok(_employeeRepository.UpdateEmployee(employee));
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id) {
            try 
            {
                return Ok(_employeeRepository.DeleteEmployee(id));
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
    }
}