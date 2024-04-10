using System.Text.Json;
using DataReceiverService.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DataReceiverService;

internal class Program {
	public static void Main(string[] args) {
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();
		builder.Services.AddHealthChecks();

		var app = builder.Build();

		if ( app.Environment.IsDevelopment() ) {
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.MapHealthChecks("/health");

		var jsonSerializerOptions = new JsonSerializerOptions {
			WriteIndented = true
		};
		app.MapPost("DataReceiver/Handle", async (UserEventsRequest userEvent, [FromServices] ILogger<Program> logger) => {
			if ( logger.IsEnabled(LogLevel.Debug) ) {
				var json = JsonSerializer.Serialize(userEvent, jsonSerializerOptions);
				logger.LogDebug("Received UserEventsRequest: {userEventsJson}", json);
			}
			return Results.Ok();
		});

		app.Run();
	}
}