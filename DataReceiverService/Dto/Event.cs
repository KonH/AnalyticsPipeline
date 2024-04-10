namespace DataReceiverService.Dto;

public record Event(Guid Id, DateTimeOffset Timestamp, string EventName, Dictionary<string, object> Properties);