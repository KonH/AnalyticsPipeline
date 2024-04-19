using System.Collections.Generic;

namespace UnityAnalyticsSDK.Abstractions {
	public interface ICustomAnalytics {
		void SendEvent(string eventName, Dictionary<string, object> parameters);
	}
}