namespace DataReceiverService;

internal class Program {
	public static void Main(string[] args) {
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddControllers();
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();
		builder.Services.AddHealthChecks();

		var app = builder.Build();

		if ( app.Environment.IsDevelopment() ) {
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseRouting();
		app.MapControllers();
		app.MapHealthChecks("/health");

		app.Run();
	}
}