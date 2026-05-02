using Azure;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using OpenAI.Chat;

namespace InterviewAssistant.Api.Agents;

public static class AgentFactory
{
    /// <summary>
    /// Creates an AIAgent backed by Azure OpenAI Chat Completion.
    /// Reads configuration from IConfiguration (appsettings.json / env vars).
    /// </summary>
    public static AIAgent CreateAzureOpenAIAgent(string name, string instructions, IConfiguration config)
    {
        var endpoint = config["AzureOpenAI:Endpoint"];
        if (string.IsNullOrWhiteSpace(endpoint))
            throw new InvalidOperationException("Missing AzureOpenAI:Endpoint configuration");

        var deployment = config["AzureOpenAI:Deployment"];
        if (string.IsNullOrWhiteSpace(deployment))
            throw new InvalidOperationException("Missing AzureOpenAI:Deployment configuration");

        var apiKey = config["AzureOpenAI:ApiKey"];

        var credential = string.IsNullOrWhiteSpace(apiKey)
            ? (object)new AzureCliCredential()
            : new AzureKeyCredential(apiKey);

        var client = credential switch
        {
            AzureCliCredential c => new AzureOpenAIClient(new Uri(endpoint), c),
            AzureKeyCredential k => new AzureOpenAIClient(new Uri(endpoint), k),
            _ => throw new InvalidOperationException("Unsupported credential")
        };

        return client
            .GetChatClient(deployment)
            .AsAIAgent(instructions: instructions, name: name);
    }
}
