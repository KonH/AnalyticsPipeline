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

app.Run();
