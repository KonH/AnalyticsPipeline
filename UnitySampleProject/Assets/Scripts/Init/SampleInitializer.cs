using UnityEngine;
using Controllers;
using Facade;

namespace Init {
	public sealed class SampleInitializer : MonoBehaviour {
		[SerializeField] SampleButtonController[] _buttons;

		void Awake() {
			var analyticsService = new AnalyticsFacade(this);
			foreach ( var button in _buttons ) {
				button.Init(analyticsService);
			}
		}
	}
}