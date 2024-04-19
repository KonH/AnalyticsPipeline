using System.Text.Json;
using Confluent.Kafka;
using DataWriterService.Model;
using Shared.Dto;

namespace DataWriterService.Services;

public sealed class QueueConsumerService : IHostedService, IDisposable {
	const int MaxRetries = 5;
	static TimeSpan DelayBetweenRetriesInSeconds => TimeSpan.FromSeconds(5);
	
	readonly ILogger<QueueConsumerService> _logger;
	readonly IConsumer<Ignore, string> _consumer;
	readonly IServiceScopeFactory _serviceScopeFactory;
	
	readonly CancellationTokenSource _stoppingCts = new();
	
	Task? _executingTask;

	public QueueConsumerService(ILogger<QueueConsumerService> logger, IServiceScopeFactory serviceScopeFactory) {
		_logger = logger;
		_serviceScopeFactory = serviceScopeFactory;
		var config = new ConsumerConfig {
			BootstrapServers = "redpanda:9092",
			GroupId = "data-writer-group",
			AutoOffsetReset = AutoOffsetReset.Earliest
		};
		_consumer = new ConsumerBuilder<Ignore, string>(config).Build();
	}

	public Task StartAsync(CancellationToken cancellationToken) {
		_executingTask = ExecuteAsync(_stoppingCts.Token);
		return Task.CompletedTask;
	}

	public async Task StopAsync(CancellationToken cancellationToken) {
		if ( _executingTask == null ) {
			return;
		}
		try {
			await _stoppingCts.CancelAsync();
		} finally {
			await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
		}
	}

	public void Dispose() {
		_stoppingCts.Cancel();
		_consumer.Dispose();
	}

	async Task ExecuteAsync(CancellationToken stoppingToken) {
		_logger.LogDebug("QueueConsumerService processing is starting.");
		int retryCount = 0;
		while ( !stoppingToken.IsCancellationRequested ) {
			try {
				// We need to call Subscribe() more than once because in these cases:
				// - Redpanda service is not yet ready on startup
				// - Redpanda service is restarted
				// - Some network issues occur
				// Exception will be thrown only from Consume() method 
				_logger.LogDebug("Subscribing to message-queue topic.");
				_consumer.Subscribe("message-queue");
				_logger.LogDebug("Consuming from Redpanda.");
				var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(60));
				_logger.LogDebug("Consumed message from Redpanda.");
				retryCount = 0;
				if ( consumeResult == null ) {
					continue;
				}
				var queueEventJson = consumeResult.Message.Value;
				QueueEvent? queueEvent;
				_logger.LogDebug("Received QueueEvent: {queueEventJson}", queueEventJson);
				try {
					queueEvent = JsonSerializer.Deserialize<QueueEvent>(queueEventJson);
					if ( queueEvent == null ) {
						_logger.LogWarning("Invalid QueueEvent received: {queueEventJson}", queueEventJson);
						continue;
					}
				} catch ( JsonException ex ) {
					_logger.LogError(ex, "Error deserializing QueueEvent: {queueEventJson}", queueEventJson);
					continue;
				}
				await HandleEvent(queueEvent, stoppingToken);
			} catch ( ConsumeException ex ) {
				_logger.LogError(ex, "Error consuming from Redpanda. Retry attempt {retryCount}", retryCount);
				retryCount++;
				if ( retryCount < MaxRetries ) {
					await Task.Delay(DelayBetweenRetriesInSeconds, stoppingToken);
				} else {
					_logger.LogError("Max retry attempts reached. Unable to consume from Redpanda.");
					throw;
				}
			}
		}
		_logger.LogDebug("QueueConsumerService processing is stopping.");
	}

	async Task HandleEvent(QueueEvent queueEvent, CancellationToken ct) {
		if ( queueEvent.EventName == "click" ) {
			_logger.LogDebug("Handling click event.");
			var buttonName = queueEvent.Properties.GetValueOrDefault("ButtonName")?.ToString() ?? string.Empty;
			var clickCount = TryGetClickCountOrDefault(queueEvent);
			var clickEvent = new ClickEventModel(
				queueEvent.Id, queueEvent.Timestamp, queueEvent.UserId, 
				buttonName, clickCount);
			using var scope = _serviceScopeFactory.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			dbContext.ClickEvents.Add(clickEvent);
			_logger.LogDebug("Saving click event to database.");
			await dbContext.SaveChangesAsync(ct);
			_logger.LogDebug("Click event saved to database.");
		} else {
			_logger.LogWarning("Unknown event: {eventId}:{eventName}", queueEvent.Id, queueEvent.EventName);
		}
	}

	int TryGetClickCountOrDefault(QueueEvent queueEvent) {
		if ( queueEvent.Properties.TryGetValue("ClickCount", out var clickCountObj) 
		     && int.TryParse(clickCountObj.ToString(), out var clickCount) ) {
			return clickCount;
		}
		_logger.LogWarning("Invalid or missing ClickCount property in event {eventId}", queueEvent.Id);
		return 0;
	}
}