using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OllamaSharp;
using System.Text;

// Initialize the Ollama API client
var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:11434") };
var ollamaClient = new OllamaApiClient(httpClient, "llama3.2");
var builder = Kernel.CreateBuilder();

#pragma warning disable SKEXP0070
builder.AddOllamaChatCompletion(ollamaClient);
#pragma warning restore SKEXP0070

var kernel = builder.Build();

var chatService = kernel.GetRequiredService<IChatCompletionService>();

var history = new ChatHistory();
history.AddSystemMessage("You are 'Linqbuilder', an expert in Microsoft Linq for .Net.");

while (true)
{
    Console.Write("You: ");
    var userMessage = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(userMessage))
    {
        break;
    }

    history.AddUserMessage(userMessage);

    var stream = chatService.GetStreamingChatMessageContentsAsync(history, new PromptExecutionSettings {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
    }, kernel);

    StringBuilder responseBuilder = new StringBuilder(string.Empty);
    await foreach (var response in stream)
    {
        responseBuilder.Append(response.Content);
        Console.Write(response.Content);
    }

    history.AddAssistantMessage(responseBuilder.ToString());
    Console.WriteLine();
}