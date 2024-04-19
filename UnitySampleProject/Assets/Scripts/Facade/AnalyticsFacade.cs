using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityAnalyticsSDK;
using UnityAnalyticsSDK.Abstractions;

namespace Facade {
	public sealed class AnalyticsFacade : IAnalyticsService {
		readonly ICustomAnalytics _customAnalytics;

		public AnalyticsFacade(MonoBehaviour context) {
			_customAnalytics = CustomAnalytics.Create(context);
		}
		
		public void SendClickEvent(string buttonName, int clickCount) {
			_customAnalytics.SendEvent("click", new Dictionary<string, object> {
				["ButtonName"] = buttonName,
				["ClickCount"] = clickCount
			});
		}
	}
}