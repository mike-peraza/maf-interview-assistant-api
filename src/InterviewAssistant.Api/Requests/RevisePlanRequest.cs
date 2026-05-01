using System.ComponentModel.DataAnnotations;
using InterviewAssistant.Api.Models;

namespace InterviewAssistant.Api.Requests;

/// <summary>Request body for POST /api/interview/plan/revise</summary>
public sealed class RevisePlanRequest
{
    [Required]
    public InterviewPlan Plan { get; set; } = new();

    /// <summary>One-sentence feedback for the planner (e.g. "more system design, fewer trivia").</summary>
    [Required, MinLength(1)]
    public string Feedback { get; set; } = "";
}
