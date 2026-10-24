using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.AI;

namespace Net11Blazor;

// Gives components access to their own circuit
public sealed class CircuitAccessor : CircuitHandler
{
    public Circuit? Circuit { get; private set; }

    public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        Circuit = circuit;
        return Task.CompletedTask;
    }
}

// Offline stand-in for a real model: streams an answer word by word.
// Replace with any IChatClient (Azure OpenAI, OpenAI, Ollama, ...) for a real agent.
public sealed class DemoChatClient : IChatClient
{
    public async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        var updates = new List<ChatResponseUpdate>();
        await foreach (var update in GetStreamingResponseAsync(messages, options, cancellationToken))
        {
            updates.Add(update);
        }
        return updates.ToChatResponse();
    }

    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var question = messages.LastOrDefault(m => m.Role == ChatRole.User)?.Text ?? "";
        // Plain paragraphs: the preview package splits text into paragraphs only.
        // Markdown (bold, lists, code) needs an app-provided mapper, e.g. with Markdig.
        var answer = string.Join("\n\n",
            $"You asked: {question}",
            "Runtime Async moves async/await from the compiler into the runtime.",
            "C# 15 adds union types, closed hierarchies, collection expression arguments, extension indexers and labeled break/continue.",
            "ASP.NET Core 11 generates OpenAPI 3.2 documents and supports the HTTP QUERY method.");

        foreach (var word in answer.Split(' '))
        {
            await Task.Delay(40, cancellationToken);
            yield return new ChatResponseUpdate(ChatRole.Assistant, word + " ");
        }
    }

    public object? GetService(Type serviceType, object? serviceKey = null) => null;

    public void Dispose()
    {
    }
}
