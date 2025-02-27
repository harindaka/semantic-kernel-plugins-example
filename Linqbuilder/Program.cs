using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.Extensions.DependencyInjection;
using OllamaSharp;
using System.Text;

// Configure the service collection
var serviceCollection = new ServiceCollection();
serviceCollection.AddSingleton<WeatherPlugin>(); // Register WeatherPlugin with the service collection

// Initialize the Ollama API client
var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:11434") };
var ollamaClient = new OllamaApiClient(httpClient, "llama3.2");

var serviceProvider = serviceCollection.BuildServiceProvider();

var builder = Kernel.CreateBuilder();

#pragma warning disable SKEXP0070
builder.AddOllamaChatCompletion(ollamaClient);
#pragma warning restore SKEXP0070

// Import the plugins from the service provider
builder.Plugins.AddFromObject(serviceProvider.GetRequiredService<WeatherPlugin>(), "WeatherPlugin");

var kernel = builder.Build();

var chatService = kernel.GetRequiredService<IChatCompletionService>();

var history = new ChatHistory();
history.AddSystemMessage(@"You are an expert Travel Planning Assistant");

while (true)
{
    Console.Write(">> ");
    var userMessage = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(userMessage))
    {
        break;
    }

    history.AddUserMessage(userMessage);

    var stream = chatService.GetStreamingChatMessageContentsAsync(history, new PromptExecutionSettings
    {
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
