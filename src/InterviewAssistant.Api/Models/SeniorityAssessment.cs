using System.Text.Json.Serialization;

namespace InterviewAssistant.Api.Models;

public sealed class SeniorityAssessment
{
    [JsonPropertyName("level")] public string Level { get; set; } = "";
    [JsonPropertyName("confidence")] public double Confidence { get; set; }
    [JsonPropertyName("rationale")] public string Rationale { get; set; } = "";
}
