using System.ComponentModel.DataAnnotations;
using InterviewAssistant.Api.Models;

namespace InterviewAssistant.Api.Requests;

/// <summary>Request body for POST /api/interview/evaluate</summary>
public sealed class EvaluateRequest
{
    [Required]
    public ResumeProfile Profile { get; set; } = new();

    [Required]
    public InterviewPlan Plan { get; set; } = new();

    /// <summary>
    /// Free-text interview notes from the interviewer.
    /// Leave empty to evaluate based on resume + plan only.
    /// </summary>
    public string Notes { get; set; } = "";
}
