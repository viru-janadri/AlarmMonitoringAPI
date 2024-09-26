using HotChocolate.Subscriptions;

public class AlarmMonitor
{
	private readonly AlarmRepository _alarmRepository;
	private readonly TimeSpan _pollingInterval;
	private List<Alarm> _currentAlarms = [];
	private readonly ITopicEventSender _eventSender;

	public AlarmMonitor(AlarmRepository alarmRepository, ITopicEventSender eventSender)
	{
		_alarmRepository = alarmRepository;
		_pollingInterval = TimeSpan.FromSeconds(5); // Poll every 5 seconds
		_eventSender = eventSender;
	}

	public async Task StartMonitoringAsync()
	{
		while (true)
		{
			Console.WriteLine("At least we are here....");

			var alarmsFromDb = await _alarmRepository.GetActiveAlarmsAsync();
			// Print initial alarms
			Console.WriteLine("Initial alarms loaded:");
			foreach (var alarm in alarmsFromDb)
			{
				Console.WriteLine($"Alarm ID: {alarm.Id}, Patient ID: {alarm.PatientId}, Label: {alarm.AlarmText}");

			}

			await PushMessagesync(alarmsFromDb);
			await Task.Delay(_pollingInterval);
		}
	}

	private async Task PushMessagesync(IEnumerable<Alarm> alarmsFromDb)
	{
		foreach (var alarm in alarmsFromDb)
		{
			await _eventSender.SendAsync(nameof(AlarmSubscriptions.OnAlarmAdded), alarm);
			Console.WriteLine($"Alarm sent to subscription: {alarm.Id}");


		}

	}

	public async Task<IEnumerable<Alarm>> GetCurrentAlarms()
	{
		// Await the result of the asynchronous call
		var currentAlarms = await _alarmRepository.GetActiveAlarmsAsync();
		return currentAlarms;
	}


}
