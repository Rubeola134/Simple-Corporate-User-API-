using API.Model;

namespace API.Data
{
    public interface IUserRepository
    {
        bool SaveChanges();

        void AddEntity<T>(T entity) where T : class;

        void Remove<T>(T entity) where T : class;

        IEnumerable<User> GetUsers();
        IEnumerable<UserSalary> GetAllUserSalaries();
        IEnumerable<UserJobInfor> GetAllUserJobInfors();

        User GetSingleUser(int userId);
        UserJobInfor GetSingleUserJobInfor(int userId);
        UserSalary GetSingleUserSalary(int userId);


    }
}