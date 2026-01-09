using API.Data;
using API.Dtos;
using API.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class UserController(IConfiguration configuration) : ControllerBase
    {
        readonly DataContextDapper _dataContextDapper = new(configuration);

        [HttpGet("GetUsers", Name = "GetTest")]
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> result = _dataContextDapper.LoadData<User>("SELECT * FROM TutorialAppSchema.Users;");

            return result;

        }

        // [HttpGet("TestConnection")]
        // public DateTime TestConnection()
        // {
        //     var result = _dataContextDapper.LoadDataSingle<DateTime>("SELECT GETDATE()");

        //     return result;
        // }

        [HttpGet("GetSingleUser/{userId}", Name = "GetSingleUser")]
        public User GetSingleUser(int userId)
        {
            var result = _dataContextDapper.LoadDataSingle<User>("SELECT * FROM TutorialAppSchema.Users WHERE UserId = " + userId.ToString() + ";");

            return result;
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            string sql = @"
            UPDATE TutorialAppSchema.Users
            SET
            FirstName = @FirstName,
            LastName  = @LastName,
            Email     = @Email,
            Gender    = @Gender,
            Active    = @Active
            WHERE UserId = @UserId;
            ";

            if (
            _dataContextDapper.ExecuteSql(sql, new
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Gender = user.Gender,
                Active = user.Active,
                UserId = user.UserId
            }))
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
            string sql = @"
INSERT INTO TutorialAppSchema.Users(
          [FirstName]
        , [LastName]
        , [Email]
        , [Gender]
        , [Active]
        ) VALUES (
            @FirstName,
            @LastName,
            @Email,
            @Gender,
            @Active
        )";

            if (
            _dataContextDapper.ExecuteSql(sql, new
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Gender = user.Gender,
                Active = user.Active,
            }))
            {
                return Ok();
            }
            else
            {
                throw new Exception("Failed to update user.");
            }

        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            string sql = @"
DELETE FROM TutorialAppSchema.Users
WHERE UserId = @UserId;
            ";

            if (
            _dataContextDapper.ExecuteSql(sql, new
            {
                UserId = userId
            }))
            {
                return Ok();
            }
            else
            {
                throw new Exception("Failed to delete user.");
            }

        }

    }
}
