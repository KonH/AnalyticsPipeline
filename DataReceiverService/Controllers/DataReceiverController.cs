using System.Text.Json;
using Confluent.Kafka;
using DataReceiverService.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DataReceiverService.Controllers;

[ApiController]
[Route("[controller]")]
public class DataReceiverController : ControllerBase {
    readonly ILogger<DataReceiverController> _logger;
    readonly JsonSerializerOptions _jsonSerializerOptions;
    readonly IProducer<Null, string> _producer;

    public DataReceiverController(ILogger<DataReceiverController> logger) {
        _logger = logger;
        _jsonSerializerOptions = new JsonSerializerOptions {
            WriteIndented = true
        };
        var config = new ProducerConfig {
            BootstrapServers = "redpanda:9092"
        };
        _logger.LogDebug("Create Kafka producer with servers: {bootstrapServers}", config.BootstrapServers);
        _producer = new ProducerBuilder<Null, string>(config)
            .Build();
    }

    [HttpPost("Handle")]
    public async Task<IActionResult> Handle(UserEventsRequest userEvent) {
        if ( _logger.IsEnabled(LogLevel.Debug) ) {
            var json = JsonSerializer.Serialize(userEvent, _jsonSerializerOptions);
            _logger.LogDebug(
                "Received UserEventsRequest with {eventCount} events: {userEventsJson}", 
                userEvent.Events.Count, json);
        }
        _logger.LogDebug("Sending {eventCount} events to message queue", userEvent.Events.Count);
        foreach ( var eventItem in userEvent.Events ) {
            _logger.LogDebug("Sending event {eventName}:{eventId} to message queue", eventItem.EventName, eventItem.Id);
            var queueEvent = new QueueEvent(eventItem.Id, eventItem.Timestamp, eventItem.EventName, eventItem.Properties, userEvent.UserId);
            var queueEventJson = JsonSerializer.Serialize(queueEvent, _jsonSerializerOptions);
            await _producer.ProduceAsync("message-queue", new Message<Null, string> {
                Value = queueEventJson
            });
            _logger.LogDebug("Event {eventName}:{eventId} to message queue sent", eventItem.EventName, eventItem.Id);
        }
        _logger.LogDebug("{eventCount} events sent to message queue", userEvent.Events.Count);
        return Ok();
    }
}