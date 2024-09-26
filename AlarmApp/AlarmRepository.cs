using Dapper;
using Npgsql;


public class AlarmRepository
{
	private readonly string _connectionString;

	public AlarmRepository(string connectionString)
	{
		_connectionString = connectionString;
	}

	public async Task<IEnumerable<Alarm>> GetActiveAlarmsAsync()
	{
		Console.WriteLine("Fetching alarms from database...");
		try
		{

			using (var db = new NpgsqlConnection(_connectionString))
			{
				await db.OpenAsync(); // Ensure connection opens correctly
				Console.WriteLine("Connection established...");

				string sqlquery = @"
				select id as id, patient_id as patient_id, label as label,
				announce_start_time as announce_start_time,
				is_acknowledged from physio.alert_state limit 10;";

				var alarms = await db.QueryAsync<AlarmDbModel>(sqlquery);

				Console.WriteLine($"Fetched {alarms.Count()} alarms from database.");


				// Map the database model to your Alarm model
				var alarmList = new List<Alarm>();

				foreach (var dbAlarm in alarms)
				{
					var alarm = new Alarm
					{
						Id = dbAlarm.id.ToString(),
						PatientId = dbAlarm.patient_id.ToString(),
						AlarmText = "MockLabel",
						Time = dbAlarm.announce_start_time ?? DateTime.UtcNow,
						IsAcknowledged = dbAlarm.is_acknowledged ?? false,
						// Hardcoded values
						Bed = "Bed A",  // Hardcode bed info
						Color = "Red",  // Hardcode color
						Kind = "Physiological", // Hardcode alarm kind
						Priority = "High"  // Hardcode priority
					};
					alarmList.Add(alarm);
				}
				Console.WriteLine("Alarms mapped successfully.");


				return alarmList;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error: {ex.Message}");

			// Mocking 10 alarms in case of an exception
			var mockAlarms = new List<Alarm>();
			for (int i = 1; i <= 2; i++)
			{
				mockAlarms.Add(new Alarm
				{
					Id = Guid.NewGuid().ToString(),
					PatientId = $"MockPatient{i}",
					AlarmText = $"Mock Alarm {i}",
					Time = DateTime.UtcNow,
					IsAcknowledged = false,
					Bed = $"Bed {i}",
					Color = "Red",
					Kind = "Physiological",
					Priority = "High"
				});
			}

			Console.WriteLine("Returning 2 mock alarms...");
			return mockAlarms;
		}
	}
}
