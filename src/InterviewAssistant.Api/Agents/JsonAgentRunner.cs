using System.Text.Json;
using Microsoft.Agents.AI;

namespace InterviewAssistant.Api.Agents;

public static class JsonAgentRunner
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    public static async Task<(T Value, string Raw)> RunJsonAsync<T>(
        AIAgent agent,
        string prompt,
        CancellationToken cancellationToken = default)
    {
        var result = await agent.RunAsync(prompt, cancellationToken: cancellationToken);
        var raw = result.Text.Trim();

        try
        {
            var value = JsonSerializer.Deserialize<T>(raw, JsonOptions)
                        ?? throw new JsonException("Deserialized null");
            return (value, raw);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Agent returned non-JSON or schema mismatch. Raw:\n{raw}", ex);
        }
    }
}
