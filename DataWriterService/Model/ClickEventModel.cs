using System.ComponentModel.DataAnnotations.Schema;

namespace DataWriterService.Model;

public class ClickEventModel(Guid id, DateTimeOffset timestamp, Guid userId, string buttonName, int clickCount) {
	[Column("id")]
	public Guid Id { get; init; } = id;
	
	[Column("ts")]
	public DateTimeOffset Timestamp { get; init; } = timestamp;
	
	[Column("user_id")]
	public Guid UserId { get; init; } = userId;
	
	[Column("button")]
	public string ButtonName { get; init; } = buttonName;
	
	[Column("clicks")]
	public int ClickCount { get; init; } = clickCount;
}