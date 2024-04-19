using System;
using System.Collections.Generic;

namespace UnityAnalyticsSDK.Internal.Dto {
	public class Event {
		public Guid Id { get; set; }
		public DateTimeOffset Timestamp { get; set; }
		public string EventName { get; set; }
		public Dictionary<string, object> Properties { get; set; }
	}
}