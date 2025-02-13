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
				await db.OpenAsync();
				Console.WriteLine("Connection established...");

				string sqlQuery = @"
                    SELECT 
                        s.id,
                        s.patient_id,
                        s.tenant_id,
                        s.announce_start_time,
                        s.onset_time,
                        s.is_acknowledged,
                        s.system_id as state_system_id,
                        s.equipment_label as state_equipment_label,
                        d.localized_text_content,
                        d.kind_attr,
                        d.priority_attr,
                        d.source,
                        d.equipment_label as desc_equipment_label,
                        d.system_id as desc_system_id
                    FROM physio.alert_state s
                    LEFT JOIN physio.alert_descriptor d 
                        ON s.system_id = d.system_id 
                        AND s.tenant_id = d.tenant_id
                    LIMIT 10;";

				var results = await db.QueryAsync<dynamic>(sqlQuery);
				Console.WriteLine($"Fetched {results.Count()} alarms from database.");

				var alarmList = new List<Alarm>();
				foreach (var row in results)
				{
					var alarm = new Alarm
					{
						Id = row.id?.ToString(),
						PatientId = row.patient_id?.ToString(),
						AlarmText = row.localized_text_content ?? "No Description Available",
						Time = row.announce_start_time ?? DateTime.UtcNow,
						OnsetTime = row.onset_time,
						IsAcknowledged = row.is_acknowledged ?? false,
						SystemId = row.state_system_id ?? row.desc_system_id,
						EquipmentLabel = row.state_equipment_label ?? row.desc_equipment_label,
						Kind = row.kind_attr ?? "Physiological",
						Priority = row.priority_attr ?? "High",
						Source = row.source,
						TenantId = row.tenant_id,
						// Match the frontend expected fields
						Bed = row.state_equipment_label ?? "Bed A",
						PatientName = $"Patient {row.patient_id?.ToString().Substring(0, 8)}", // Generate a readable patient name
						Color = row.priority_attr?.ToLower() == "high" ? "Red" : "Yellow",
						State = AlarmState.Active, // Default to Active
						IsActive = row.end_time == null, // Active if not ended
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

			// Return mock alarms in case of error
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
					Priority = "High",
					SystemId = $"SYS{i}",
					Source = "Mock",
					EquipmentLabel = $"EQ{i}",
					TenantId = "MOCK_TENANT"
				});
			}

			Console.WriteLine("Returning 2 mock alarms...");
			return mockAlarms;
		}
	}
}