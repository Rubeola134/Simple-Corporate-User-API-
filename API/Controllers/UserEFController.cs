using API.Data;
using API.Dtos;
using API.Model;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class UserEFController(IUserRepository userRepository) : ControllerBase
    {
        readonly IUserRepository _userRepository = userRepository;

        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> result = _userRepository.GetUsers();
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
            return _userRepository.GetSingleUser(userId);
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            User? userDb = _userRepository.GetSingleUser(user.UserId) ?? throw new Exception("User not found.");
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            userDb.Active = user.Active;
            if (_userRepository.SaveChanges())
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
            _userRepository.AddEntity(userDb);
            if (_userRepository.SaveChanges())
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
            User? userDb = _userRepository.GetSingleUser(userId) ?? throw new Exception("User not found.");
            _userRepository.Remove(userDb);
            if (_userRepository.SaveChanges())
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
           return _userRepository.GetSingleUserJobInfor(userId);
        }
        [HttpGet("GetUserSalary/{userId}")]
        public UserSalary GetUserSalary(int userId)
        {
            return _userRepository.GetSingleUserSalary(userId);
        }

        [HttpGet("GetAllUserSalaries")]
        public IEnumerable<UserSalary> GetAllUserSalaries()
        {
           return _userRepository.GetAllUserSalaries() ?? throw new Exception("No User Salaries found.");
        }
        [HttpGet("GetAllUserJobInfors")]
        public IEnumerable<UserJobInfor> GetAllUserJobInfors()
        {
            return _userRepository.GetAllUserJobInfors() ?? throw new Exception("No User Job Infors found.");
        }

        [HttpPost("AddUserJobInfor")]
        public IActionResult AddUserJobInfor(UserJobInforDto userJobInforDto)
        {
            UserJobInfor userJobInforDb = new UserJobInfor()
            {
                JobTitle = userJobInforDto.JobTitle,
                Department = userJobInforDto.Department
            };
            _userRepository.AddEntity(userJobInforDb);
            if (_userRepository.SaveChanges())
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
            _userRepository.AddEntity(userSalaryDb);
            if (_userRepository.SaveChanges())
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
            UserJobInfor? userJobInforDb = _userRepository.GetSingleUserJobInfor(userJobInfor.UserId);
            userJobInforDb.JobTitle = userJobInfor.JobTitle;
            userJobInforDb.Department = userJobInfor.Department;
            if (_userRepository.SaveChanges())
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
            UserSalary? userSalaryDb = _userRepository.GetSingleUserSalary(userSalary.UserId);
            if (_userRepository.SaveChanges())
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
            UserJobInfor? userJobInforDb = _userRepository.GetSingleUserJobInfor(userId); ;
            _userRepository.Remove(userJobInforDb);
            if (_userRepository.SaveChanges())
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
            UserSalary? userSalaryDb = _userRepository.GetSingleUserSalary(userId); ;
            _userRepository.Remove(userSalaryDb);
            if (_userRepository.SaveChanges())
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
