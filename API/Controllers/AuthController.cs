using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Dtos;
using API.Helpers;
using API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;


namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IConfiguration configuration) : ControllerBase
    {
        private readonly DataContextDapper _dataContextDapper = new(configuration);
        private readonly AuthHelper _authHelper = new(configuration);

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto userForRegistration)
        {
            // 1) basic validation
            if (string.IsNullOrWhiteSpace(userForRegistration.Email) ||
                string.IsNullOrWhiteSpace(userForRegistration.Password) ||
                string.IsNullOrWhiteSpace(userForRegistration.PasswordConfirm))
            {
                return BadRequest("Email and password are required.");
            }

            if (userForRegistration.Password != userForRegistration.PasswordConfirm)
            {
                return BadRequest("Passwords do not match.");
            }

            // Normalize email (simple version)
            string email = userForRegistration.Email.Trim().ToLowerInvariant();

            // 2) check if email exists (SAFE parameterized SQL)
            const string sqlExists = @"
SELECT CASE WHEN EXISTS (
    SELECT 1
    FROM TutorialAppSchema.Auth
    WHERE Email = @Email
) THEN 1 ELSE 0 END;";

            bool exists = _dataContextDapper.LoadDataSingle<int>(sqlExists, new { Email = email }) == 1;

            if (exists)
            {
                return BadRequest("User with this email already exists.");
            }

            // 3) create salt + hash (PBKDF2)
            byte[] salt = new byte[16]; // 128-bit salt
            RandomNumberGenerator.Fill(salt);

            // "Pepper" (secret) stored in appsettings / user-secrets / env var
            string pepper = configuration["AppSettings:PasswordKey"] ?? "";

            // Combine pepper into the password input
            string passwordWithPepper = userForRegistration.Password + pepper;

            byte[] hash = KeyDerivation.Pbkdf2(
                password: passwordWithPepper,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100_000,     // tune later; keep consistent
                numBytesRequested: 32        // 256-bit hash
            );

            // 4) insert user (SAFE parameterized SQL)
            const string sqlInsert = @"
INSERT INTO TutorialAppSchema.Auth (Email, PasswordHash, PasswordSalt)
VALUES (@Email, @PasswordHash, @PasswordSalt);";

            int rows = _dataContextDapper.ExecuteSqlwithRowCount(sqlInsert, new
            {
                Email = email,
                PasswordHash = hash,  // varbinary in SQL
                PasswordSalt = salt   // varbinary in SQL
            });

            if (rows > 0)
            {
                string sqlAddUser = @"
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
                    _dataContextDapper.ExecuteSql(sqlAddUser, new
                    {
                        FirstName = userForRegistration.FirstName,
                        LastName = userForRegistration.LastName,
                        Email = userForRegistration.Email,
                        Gender = userForRegistration.Gender,
                        Active = true,
                    })) { return Ok("Registered."); }
            }

            return BadRequest("Failed to register user.");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            // 1) basic validation
            if (string.IsNullOrWhiteSpace(userForLogin.Email) ||
                string.IsNullOrWhiteSpace(userForLogin.Password))
            {
                return BadRequest("Email and password are required.");
            }

            string email = userForLogin.Email.Trim().ToLowerInvariant();
            // 2) retrieve user by email (SAFE parameterized SQL)
            const string sqlSelect = @"
            SELECT * FROM TutorialAppSchema.Auth
            WHERE Email = @Email;";
            AuthForLoginDto? user = _dataContextDapper.LoadDataSingle<AuthForLoginDto>(sqlSelect, new { Email = email });
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            // 3) compute hash with stored salt and compare
            byte[] storedSalt = user.PasswordSalt;
            string pepper = configuration["AppSettings:PasswordKey"] ?? "";
            string passwordWithPepper = userForLogin.Password + pepper;
            byte[] computedHash = KeyDerivation.Pbkdf2(
                password: passwordWithPepper,
                salt: storedSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100_000,
                numBytesRequested: 32
            );
            // 4) compare hashes
            if (!computedHash.SequenceEqual(user.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }
            
            int userId = _dataContextDapper.LoadDataSingle<int>(
                "SELECT UserId FROM TutorialAppSchema.Users WHERE Email = @Email;",
                new { Email = email });

            return Ok(new Dictionary<string, string>
            {
                {"token", _authHelper.CreateToken(new User { UserId = userId, Email = email }) }
            });
        }

        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken()
        {
            string userId = User.FindFirst("userId")?.Value ?? throw new Exception("UserId claim missing");

            string userIdSql = @"
            SELECT UserId, Email FROM TutorialAppSchema.Users WHERE UserId = @UserId;";
            User userIdFromDb = _dataContextDapper.LoadDataSingle<User>(userIdSql, new { UserId = int.Parse(userId) });
            
            return Ok(new Dictionary<string, string>
            {
                {"token", _authHelper.CreateToken(userIdFromDb) }
            });

        }



    }


}