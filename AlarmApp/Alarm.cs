public class Alarm
{
    internal bool IsDelivered = false;
    internal bool IsAccepted = false;

    public string Bed { get; set; } = "Bed A";
    public string PatientName { get; set; } = "John Doe";
    public string? AlarmText { get; set; } // Map from "label" column
    public string? PatientId { get; set; } // Map from "patient_id" column
    public DateTime Time { get; set; } // Map from "announce_start_time" column
    public AlarmState State { get; set; } = AlarmState.Active; // Hardcoded default
    public string Color { get; set; } = "Red";
    public string? Id { get; set; } // Map from "id" column
    public string Kind { get; set; } = "Physiological";
    public string Priority { get; set; } = "High";
    public bool IsActive { get; set; } = true;
    public bool IsAcknowledged { get; set; } // Map from "is_acknowledged" column
}

public enum AlarmState
{
    Active,
    Sending,
    Delivered,
    Accepted,
    Acknowledged,
    Exported
}
