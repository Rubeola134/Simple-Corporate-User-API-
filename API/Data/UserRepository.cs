using API.Model;

namespace API.Data
{
    public class UserRepository(IConfiguration configuration) : IUserRepository
    {
        readonly DataContextEF _entityFramework = new(configuration);

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entity) where T : class
        {
            _entityFramework.Add(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            _entityFramework.Remove(entity);
        }

        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> result = _entityFramework.Users.ToList<User>();
            return result;
        }
        public User GetSingleUser(int userId)
        {
            User? result = _entityFramework.Users.Where<User>(u => u.UserId == userId).FirstOrDefault<User>();
            if (result != null)
            {
                return result;
            }
            throw new Exception("User not found.");
        }
        public UserJobInfor GetSingleUserJobInfor(int userId)
        {
            UserJobInfor? result = _entityFramework.UserJobInfors.Where<UserJobInfor>(u => u.UserId == userId).FirstOrDefault<UserJobInfor>() ?? throw new Exception("User Job Infor not found.");

            return result;
        }
        public UserSalary GetSingleUserSalary(int userId)
        {
            UserSalary? result = _entityFramework.UserSalaries.Where<UserSalary>(u => u.UserId == userId).FirstOrDefault<UserSalary>() ?? throw new Exception("User Salary not found.");

            return result;
        }

        public IEnumerable<UserSalary> GetAllUserSalaries()
        {
            IEnumerable<UserSalary> result = _entityFramework.UserSalaries.ToList<UserSalary>() ?? throw new Exception("No User Salaries found.");
            return result;
        }

        public IEnumerable<UserJobInfor> GetAllUserJobInfors()
        {
            IEnumerable<UserJobInfor> result = _entityFramework.UserJobInfors.ToList<UserJobInfor>() ?? throw new Exception("No User Job Infors found.");
            return result;
        }

    }
}