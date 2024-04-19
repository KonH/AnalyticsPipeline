using System;
using System.Collections.Generic;

namespace UnityAnalyticsSDK.Internal.Dto {
	public class UserEventsRequest {
		public Guid UserId { get; set; }
		public List<Event> Events { get; set; }
	}
}