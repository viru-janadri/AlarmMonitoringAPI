public class AlarmSubscriptions
{
	[Subscribe]
	[Topic]
	public Alarm OnAlarmAdded([EventMessage] Alarm alarm) => alarm;

	[Subscribe]
	[Topic]
	public Alarm OnAlarmUpdated([EventMessage] Alarm alarm) => alarm;

	[Subscribe]
	[Topic]
	public Alarm OnAlarmRemoved([EventMessage] Alarm alarm) => alarm;
}
