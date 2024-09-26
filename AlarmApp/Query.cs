public class Query
{
	private readonly AlarmMonitor _alarmMonitor;

	public Query(AlarmMonitor alarmMonitor)
	{
		_alarmMonitor = alarmMonitor;
	}

	// This query method returns a Task<IEnumerable<Alarm>> and awaits GetCurrentAlarms
	public async Task<IEnumerable<Alarm>> GetAlarms()
	{
		return await _alarmMonitor.GetCurrentAlarms();
	}
}
