using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityAnalyticsSDK.Abstractions;
using UnityAnalyticsSDK.Internal.Dto;

using Event = UnityAnalyticsSDK.Internal.Dto.Event;

namespace UnityAnalyticsSDK.Internal {
	/// <summary>
	/// Simplified version of analytics SDK, lacks of customization, persistence and error handling
	/// </summary>
	sealed class AnalyticsService: ICustomAnalytics {
		const string Endpoint = "http://localhost:8081/DataReceiver/Handle";
		
		readonly Guid _userId = Guid.NewGuid();

		readonly MonoBehaviour _context;

		public AnalyticsService(MonoBehaviour context) {
			_context = context;
		}
		
		public void SendEvent(string eventName, Dictionary<string, object> parameters) {
			var eventData = new UserEventsRequest {
				UserId = _userId,
				Events = new List<Event>() {
					new Event() {
						Id = Guid.NewGuid(),
						Timestamp = DateTimeOffset.UtcNow,
						EventName = eventName,
						Properties = parameters
					}
				}
			};
			var json = JsonConvert.SerializeObject(eventData);
			_context.StartCoroutine(SendData(json));
		}

		static IEnumerator SendData(string jsonData) {
			Debug.Log($"Sending analytics event, json: {jsonData}");
			using ( var webRequest = UnityWebRequest.Post(Endpoint, jsonData, "application/json") ) {
				yield return webRequest.SendWebRequest();
				if ( webRequest.result != UnityWebRequest.Result.Success ) {
					Debug.LogError($"Analytics event failed to send: {webRequest.error}, text: {webRequest.downloadHandler.text}");
				} else {
					Debug.Log("Analytics event sent successfully");
				}
			}
		}
	}
}