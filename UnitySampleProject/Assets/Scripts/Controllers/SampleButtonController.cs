using UnityEngine;
using UnityEngine.UI;
using Core;

namespace Controllers {
	public sealed class SampleButtonController : MonoBehaviour {
		[SerializeField] string _buttonName;
		[SerializeField] Button _button;
		
		IAnalyticsService _analyticsService;

		int _clickCount;
		
		void Awake() {
			_button.onClick.AddListener(OnButtonClick);
		}
		
		public void Init(IAnalyticsService analyticsService) {
			_analyticsService = analyticsService;
		}

		void OnButtonClick() {
			_clickCount++;
			_analyticsService.SendClickEvent(_buttonName, _clickCount);
		}
	}
}