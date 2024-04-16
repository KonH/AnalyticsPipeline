using System.Text.Json;
using DataReceiverService.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DataReceiverService.Controllers;

[ApiController]
[Route("[controller]")]
public class DataReceiverController : ControllerBase {
    readonly ILogger<DataReceiverController> _logger;
    readonly JsonSerializerOptions _jsonSerializerOptions;

    public DataReceiverController(ILogger<DataReceiverController> logger) {
        _logger = logger;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
    }

    [HttpPost("Handle")]
    public IActionResult Handle(UserEventsRequest userEvent) {
        if ( _logger.IsEnabled(LogLevel.Debug) ) {
            var json = JsonSerializer.Serialize(userEvent, _jsonSerializerOptions);
            _logger.LogDebug("Received UserEventsRequest: {userEventsJson}", json);
        }
        return Ok();
    }
}