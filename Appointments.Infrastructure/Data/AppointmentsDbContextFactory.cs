using Appointments.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyProject.Infrastructure.Data
{
    public class AppointmentsDbContextFactory : IDesignTimeDbContextFactory<AppointmentsDbContext>
    {
        public AppointmentsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppointmentsDbContext>();

             
            var connectionString = "Server=localhost;Database=appointmentsDb;Trusted_Connection=True;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);

            return new AppointmentsDbContext(optionsBuilder.Options);
        }
    }
}
