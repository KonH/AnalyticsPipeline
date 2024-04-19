using DataWriterService.Services;

namespace DataWriterService;

internal class Program {
	public static void Main(string[] args) {
		var builder = WebApplication.CreateBuilder(args);

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