public record AiServiceConfig(string ModelId, string Url, int TimeoutSeconds = 30);
public sealed record AzureOpenAiConfig(string ModelId, string Url, string ApiKey) : AiServiceConfig(ModelId, Url);
public sealed record AiServicesConfig(AzureOpenAiConfig AzureOpenAi, AiServiceConfig Ollama);