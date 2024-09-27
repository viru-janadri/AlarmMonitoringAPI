// Class to map database columns
public class AlarmDbModel
{
	public Guid id { get; set; }
	public Guid? patient_id { get; set; }
	public string? label { get; set; }
	public DateTime? announce_start_time { get; set; }
	public bool? is_acknowledged { get; set; }
}
