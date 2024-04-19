using Microsoft.EntityFrameworkCore;

namespace DataWriterService.Model;

public class AppDbContext : DbContext {
	public DbSet<ClickEventModel> ClickEvents { get; set; }
	
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
}