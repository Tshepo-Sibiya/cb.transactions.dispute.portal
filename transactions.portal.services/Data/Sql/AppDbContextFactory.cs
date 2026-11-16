using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace transactions.portal.services.Data.Sql;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=DisputePortal;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;");
        return new AppDbContext(optionsBuilder.Options);
    }
}

