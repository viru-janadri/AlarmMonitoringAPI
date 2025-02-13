public class Alarm
{
    internal bool IsDelivered = false;
    internal bool IsAccepted = false;

    public string Bed { get; set; } = "Bed A";
    public string PatientName { get; set; } = "John Doe";
    public string? AlarmText { get; set; } // Map from alert_descriptor.localized_text_content
    public string? PatientId { get; set; } // Map from alert_state.patient_id
    public DateTime Time { get; set; } // Map from alert_state.announce_start_time
    public AlarmState State { get; set; } = AlarmState.Active;
    public string Color { get; set; } = "Red";
    public string? Id { get; set; } // Map from alert_state.id
    public string Kind { get; set; } = "Physiological"; // Map from alert_descriptor.kind_attr
    public string Priority { get; set; } = "High"; // Map from alert_descriptor.priority_attr
    public bool IsActive { get; set; } = true;
    public bool IsAcknowledged { get; set; } // Map from alert_state.is_acknowledged
    public string? SystemId { get; set; } // Map from alert_state.system_id
    public string? Source { get; set; } // Map from alert_descriptor.source
    public string? EquipmentLabel { get; set; } // Map from either table
    public DateTime? OnsetTime { get; set; } // Map from alert_state.onset_time
    public string? TenantId { get; set; } // Map from either table
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