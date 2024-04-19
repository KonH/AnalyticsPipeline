using DataWriterService.Model;
using DataWriterService.Services;
using Microsoft.EntityFrameworkCore;

namespace DataWriterService;

internal class Program {
	public static void Main(string[] args) {
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddDbContext<AppDbContext>(options =>
			options.UseNpgsql("Host=postgres;Database=analytics_database;Username=analytics_rw_user;Password=analytics_rw_user_password"));
		
		builder.Services.AddControllers();
		builder.Services.AddHealthChecks();
		builder.Services.AddHostedService<QueueConsumerService>();

		var app = builder.Build();

		app.UseRouting();
		app.MapControllers();
		app.MapHealthChecks("/health");

		app.Run();
	}
}