using UnityEngine;
using UnityAnalyticsSDK.Abstractions;
using UnityAnalyticsSDK.Internal;

namespace UnityAnalyticsSDK {
	public static class CustomAnalytics {
		public static ICustomAnalytics Create(MonoBehaviour context) {
			return new AnalyticsService(context);
		}
	}
}