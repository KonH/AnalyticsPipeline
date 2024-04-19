namespace Core {
	public interface IAnalyticsService {
		void SendClickEvent(string buttonName, int clickCount);
	}
}