using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


var modelId = "llama3.2";
var endpoint = new Uri("http://localhost:11434");

var builder = Kernel.CreateBuilder();
var services = builder.Services;

services
    .AddSingleton<ILoggerFactory, LoggerFactory>()
    .AddSingleton(typeof(ILogger<>), typeof(Logger<>))
    .AddLogging(builder => builder.AddDebug().SetMinimumLevel(LogLevel.Debug));

// Configure HTTP client with increased timeout
// as the first request to the Ollama API usually takes a while.
var httpClient = new HttpClient
{
    BaseAddress = endpoint,
    Timeout = TimeSpan.FromMinutes(5)
};

#pragma warning disable SKEXP0070
builder.Services.AddOllamaChatCompletion(modelId, httpClient);
#pragma warning restore SKEXP0070

builder.Plugins.AddFromType<WeatherPlugin>();
builder.Plugins.AddFromType<FlightsPlugin>();
builder.Plugins.AddFromType<HotelsPlugin>();
builder.Plugins.AddFromType<CurrencyPlugin>();

var kernel = builder.Build();
var chatService = kernel.GetRequiredService<IChatCompletionService>();

#pragma warning disable SKEXP0070
var settings = new OllamaPromptExecutionSettings { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };
#pragma warning restore SKEXP0070

var chatHistory = new ChatHistory();

//A little bit of prompt engineering
chatHistory.AddSystemMessage($@"
You are a travel planning assistant. You help users find flights, book hotels check the weather and check currency conversion rates.

You have at your disposal the following plugins:
- Weather - provides weather information for a given city. Do not invoke this function unless a city and a date is explicitly specified by the user, instead you MUST ask the user for the missing information.
- Flights - provides flight information for a given date, origin and destination airport. Do not invoke this function unless a date, origin and destination is explicitly specified by the user, instead you MUST ask the user for the missing information.
- Hotels - provides hotel information for a given city. Do not invoke this function unless a city is explicitly specified by the user, instead you MUST ask the user for the missing information.
- Currency Conversion - provides the current currency conversion rates for a given country, source and target currency code. Note that users may specify the currency using the ISO 4217 standard currency code which you will need to pass on to the plugin. Do not invoke this function unless a country, source and target currency is explicitly specified by the user, instead you MUST ask the user for the missing information.
With each response, you should provide the user with a clear and concise answer to their query, and you MUST ask for any missing information.

DO NOT offer to retrieve the same information outside of the plugin calls.
You MUST greet the user and introduce yourself as a travel planning assistant when the user greets you for the first time and explain how you can help the user using the aforementioned services at your disposal.
You MUST only discuss matters related to travel planning.
WHEN you do not know how to answer or IF the conversation goes astray, you MUST always respond by suggesting that you have access to the aforementioned services and offer to help.
You MUST NEVER show the plugin results in JSON format. Instead you MUST ALWAYS provide a human-readable response to the user.
IF you make a mistake, you should let the user know that you did not understand the query and ask the user to rephrase the question. You MUST NOT repeatedly mention that you made a mistake which may annoy the user.
WHEN the user bids farewell, you MUST bid farewell to the user and thank them for using your services.
");

while (true)
{    
    
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("You: ");    
    Console.ResetColor();
    var userMessage = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userMessage))
    {
        break;
    }

    chatHistory.AddUserMessage(userMessage);

    // Unfortunately, streaming function calls seems to be not supported by the Ollama API at the moment.
    // https://github.com/microsoft/semantic-kernel/issues/9988#issuecomment-2572786890
    // Thus we have to use the synchronous version of this.
    // var stream = chatService.GetStreamingChatMessageContentsAsync(chatHistory, settings, kernel);    
    // StringBuilder responseBuilder = new StringBuilder();
    // await foreach (var response in stream)
    // {
    //     responseBuilder.Append(response.Content);
    //     Console.Write(response.Content);
    // }

    // chatHistory.AddAssistantMessage(responseBuilder.ToString());

    var chatResponse = await chatService.GetChatMessageContentAsync(chatHistory, settings, kernel);

    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine("Assistant: ");
    Console.ResetColor();
    Console.Write(chatResponse.Content);
    Console.WriteLine();

    chatHistory.AddAssistantMessage(chatResponse.Content ?? string.Empty);
    
    Console.WriteLine();
}