using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataWriterService.Model;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext> {
	public AppDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
		optionsBuilder.UseNpgsql("Host=postgres;Database=analytics_database;Username=analytics_rw_user;Password=analytics_rw_user_password");

		return new AppDbContext(optionsBuilder.Options);
	}
}