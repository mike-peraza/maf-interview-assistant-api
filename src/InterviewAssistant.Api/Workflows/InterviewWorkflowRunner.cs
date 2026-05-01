using System.Text;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;

namespace InterviewAssistant.Api.Workflows;

public static class InterviewWorkflowRunner
{
    /// <summary>
    /// Runs a sequential workflow: ResumeIngestion -> SeniorityClassifier -> InterviewPlanner.
    /// Returns the planner's raw JSON and per-executor debug output.
    /// </summary>
    public static async Task<(string PlannerRawJson, Dictionary<string, string> PerExecutorOutput)> RunPlanWorkflowAsync(
        AIAgent resumeIngestionAgent,
        AIAgent seniorityAgent,
        AIAgent plannerAgent,
        ChatMessage input,
        CancellationToken cancellationToken = default)
    {
        var workflow = new WorkflowBuilder(resumeIngestionAgent)
            .AddEdge(resumeIngestionAgent, seniorityAgent)
            .AddEdge(seniorityAgent, plannerAgent)
            .Build();

        var buffers = new Dictionary<string, StringBuilder>(StringComparer.OrdinalIgnoreCase);

        await using StreamingRun run = await InProcessExecution.StreamAsync(workflow, input, cancellationToken: cancellationToken);
        await run.TrySendMessageAsync(new TurnToken(emitEvents: true));

        await foreach (WorkflowEvent evt in run.WatchStreamAsync(cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (evt is AgentResponseUpdateEvent update)
            {
                if (!buffers.TryGetValue(update.ExecutorId, out var sb))
                {
                    sb = new StringBuilder();
                    buffers[update.ExecutorId] = sb;
                }
                sb.Append(update.Data);
            }
        }

        var outputs = buffers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());

        var plannerKey = outputs.Keys.LastOrDefault()
                         ?? throw new InvalidOperationException("No workflow output captured.");

        return (outputs[plannerKey].Trim(), outputs);
    }
}
