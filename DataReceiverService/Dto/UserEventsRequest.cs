namespace DataReceiverService.Dto;

public record UserEventsRequest(Guid UserId, List<Event> Events);