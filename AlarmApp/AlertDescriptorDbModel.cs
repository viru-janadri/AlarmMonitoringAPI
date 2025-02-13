public class AlertDescriptorDbModel
{
    public string? text_id { get; set; }
    public int? alert_source { get; set; }
    public string? system_id { get; set; }
    public string? sdc_type { get; set; }
    public string? localized_text_content { get; set; }
    public string? lang_attr { get; set; }
    public string? code_attr { get; set; }
    public string? coding_system_attr { get; set; }
    public string? symbolic_code_name_attr { get; set; }
    public string? handle_attr { get; set; }
    public decimal? descriptor_version_attr { get; set; }
    public string? safety_classification_attr { get; set; }
    public string? source { get; set; }
    public string? kind_attr { get; set; }
    public string? priority_attr { get; set; }
    public decimal? default_condition_generation_delay_attr { get; set; }
    public int? timestamp { get; set; }
    public string? equipment_label { get; set; }
    public Guid? id { get; set; }
    public string? tenant_id { get; set; }
    public DateTime? ingestion_time { get; set; }
}