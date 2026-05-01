using System.ComponentModel.DataAnnotations;

namespace InterviewAssistant.Api.Requests;

/// <summary>Request body for POST /api/interview/analyze</summary>
public sealed class AnalyzeResumeRequest
{
    /// <summary>Plain-text resume content.</summary>
    [Required, MinLength(1)]
    public string ResumeText { get; set; } = "";

    /// <summary>Target role for context (e.g. "Software Engineer").</summary>
    public string Role { get; set; } = "Software Engineer";
}
