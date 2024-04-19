namespace Shared.Dto;

public record QueueEvent(Guid Id, DateTimeOffset Timestamp, string EventName, Dictionary<string, object> Properties, Guid UserId);