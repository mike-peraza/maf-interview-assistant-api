using System.ComponentModel.DataAnnotations;
using InterviewAssistant.Api.Models;

namespace InterviewAssistant.Api.Requests;

/// <summary>Request body for POST /api/interview/plan</summary>
public sealed class GeneratePlanRequest
{
    [Required]
    public ResumeProfile Profile { get; set; } = new();

    [Required]
    public SeniorityAssessment Seniority { get; set; } = new();

    /// <summary>Target role (e.g. "Software Engineer").</summary>
    public string Role { get; set; } = "Software Engineer";

    /// <summary>Execution mode: "simple" (default) or "workflow".</summary>
    public string Mode { get; set; } = "simple";

    /// <summary>Original resume text — required for workflow mode.</summary>
    public string? ResumeText { get; set; }
}
