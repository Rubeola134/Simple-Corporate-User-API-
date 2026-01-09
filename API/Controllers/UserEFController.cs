using API.Data;
using API.Dtos;
using API.Model;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class UserEFController(IConfiguration configuration) : ControllerBase
    {
        readonly DataContextEF _entityFramework = new(configuration);

        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> result = _entityFramework.Users.ToList<User>();
            return result;
        }

        // [HttpGet("TestConnection")]
        // public DateTime TestConnection()
        // {
        //     var result = _dataContextDapper.LoadDataSingle<DateTime>("SELECT GETDATE()");

        //     return result;
        // }

        [HttpGet("GetSingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {
            User? result = _entityFramework.Users.Where<User>(u => u.UserId == userId).FirstOrDefault<User>();
            if (result != null)
            {
                return result;
            }
            throw new Exception("User not found.");
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            User? userDb = _entityFramework.Users.Where<User>(u => u.UserId == user.UserId).FirstOrDefault<User>() ?? throw new Exception("User not found.");
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            userDb.Active = user.Active;
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            else
            {
                throw new Exception("Failed to update user.");
            }

        }

        [HttpPost("AddUser")]

        public IActionResult AddUser(UserToAddDto user)
        {
            User userDb = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Gender = user.Gender,
                Active = user.Active
            };
            _entityFramework.Users.Add(userDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            else
            {
                throw new Exception("Failed to add user.");
            }
        }
        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            User? userDb = _entityFramework.Users.Where<User>(u => u.UserId == userId).FirstOrDefault<User>() ?? throw new Exception("User not found.");
            _entityFramework.Users.Remove(userDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            else
            {
                throw new Exception("Failed to delete user.");
            }

        }

        [HttpGet("GetUserJobInfor/{userId}")]
        public UserJobInfor GetUserJobInfor(int userId)
        {
            UserJobInfor? result = _entityFramework.UserJobInfors.Where<UserJobInfor>(u => u.UserId == userId).FirstOrDefault<UserJobInfor>() ?? throw new Exception("User Job Infor not found.");

            return result;
        }
        [HttpGet("GetUserSalary/{userId}")]
        public UserSalary GetUserSalary(int userId)
        {
            UserSalary? result = _entityFramework.UserSalaries.Where<UserSalary>(u => u.UserId == userId).FirstOrDefault<UserSalary>() ?? throw new Exception("User Salary not found.");

            return result;
        }

        [HttpGet("GetAllUserSalaries")]
        public IEnumerable<UserSalary> GetAllUserSalaries()
        {
            IEnumerable<UserSalary> result = _entityFramework.UserSalaries.ToList<UserSalary>() ?? throw new Exception("No User Salaries found.");
            return result;
        }
        [HttpGet("GetAllUserJobInfors")]
        public IEnumerable<UserJobInfor> GetAllUserJobInfors()
        {
            IEnumerable<UserJobInfor> result = _entityFramework.UserJobInfors.ToList<UserJobInfor>() ?? throw new Exception("No User Job Infors found.");
            return result;
        }

        [HttpPost("AddUserJobInfor")]
        public IActionResult AddUserJobInfor(UserJobInforDto userJobInforDto)
        {
            UserJobInfor userJobInforDb = new UserJobInfor()
            {
                JobTitle = userJobInforDto.JobTitle,
                Department = userJobInforDto.Department
            };
            _entityFramework.UserJobInfors.Add(userJobInforDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            else
            {
                throw new Exception("Failed to add user job infor.");
            }
        }

        [HttpPost("AddUserSalary")]
        public IActionResult AddUserSalary(UserSalaryDto userSalary)
        {
            UserSalary userSalaryDb = new UserSalary()
            {
                Salary = userSalary.Salary
            };
            _entityFramework.UserSalaries.Add(userSalaryDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            else
            {
                throw new Exception("Failed to add user salary.");
            }
        }

        [HttpPut("EditUserJobInfor")]
        public IActionResult EditUserJobInfor(UserJobInfor userJobInfor)
        {
            UserJobInfor? userJobInforDb = _entityFramework.UserJobInfors.Where<UserJobInfor>(u => u.UserId == userJobInfor.UserId).FirstOrDefault<UserJobInfor>() ?? throw new Exception("User Job Infor not found.");
            userJobInforDb.JobTitle = userJobInfor.JobTitle;
            userJobInforDb.Department = userJobInfor.Department;
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            else
            {
                throw new Exception("Failed to update user job infor.");
            }
        }
        [HttpPut("EditUserSalary")]
        public IActionResult EditUserSalary(UserSalary userSalary)
        {
            UserSalary? userSalaryDb = _entityFramework.UserSalaries.Where<UserSalary>(u => u.UserId == userSalary.UserId).FirstOrDefault<UserSalary>() ?? throw new Exception("User Salary not found.");
            userSalaryDb.Salary = userSalary.Salary;
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            else
            {
                throw new Exception("Failed to update user salary.");
            }
        }
        [HttpDelete("DeleteUserJobInfor/{userId}")]
        public IActionResult DeleteUserJobInfor(int userId)
        {
            UserJobInfor? userJobInforDb = _entityFramework.UserJobInfors.Where<UserJobInfor>(u => u.UserId == userId).FirstOrDefault<UserJobInfor>() ?? throw new Exception("User Job Infor not found.");
            _entityFramework.UserJobInfors.Remove(userJobInforDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            else
            {
                throw new Exception("Failed to delete user job infor.");
            }
        }
        [HttpDelete("DeleteUserSalary/{userId}")]
        public IActionResult DeleteUserSalary(int userId)
        {
            UserSalary? userSalaryDb = _entityFramework.UserSalaries.Where<UserSalary>(u => u.UserId == userId).FirstOrDefault<UserSalary>() ?? throw new Exception("User Salary not found.");
            _entityFramework.UserSalaries.Remove(userSalaryDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            else
            {
                throw new Exception("Failed to delete user salary.");
            }
        }
    }
}
