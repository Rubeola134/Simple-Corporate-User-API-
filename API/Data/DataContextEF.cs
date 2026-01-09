namespace API.Data
{
    using API.Model;
    using Microsoft.EntityFrameworkCore;

    public class DataContextEF(IConfiguration configuration) : DbContext
    {
        private readonly IConfiguration _configuration = configuration;

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserJobInfor> UserJobInfors { get; set; }
        public virtual DbSet<UserSalary> UserSalaries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                _configuration.GetConnectionString("DefaultConnection")
            );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TutorialAppSchema");
            modelBuilder.Entity<User>().ToTable("Users", "TutorialAppSchema").HasKey(u => u.UserId);
            modelBuilder.Entity<UserJobInfor>().ToTable("UserJobInfors", "TutorialAppSchema").HasKey(uj => uj.UserId);
            modelBuilder.Entity<UserSalary>().ToTable("UserSalaries", "TutorialAppSchema").HasKey(us => us.UserId);
        }

    }
}
