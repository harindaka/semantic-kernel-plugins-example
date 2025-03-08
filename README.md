
# Extending AI with Semantic Kernel Functions

This project demonstrates the fundamentals of building your own copilot with the use of a Azure OpenAI or Ollama (via a locally hosted Llama3.2 model) in conjunction with Microsoft Semantic Kernel Plugins. Semantic Kernel provides means to extend the capabilities of AI by getting it to invoke custom code in your application based on prompt engineering and user input / queries. Watch the video below to see it in action:

[![Watch the video](https://img.youtube.com/vi/HzMQPx6Xhzw/0.jpg)](https://www.youtube.com/watch?v=HzMQPx6Xhzw)

## What is Semantic Kernel?

Microsoft Semantic Kernel is a framework that enables developers to build AI applications using natural language processing (NLP) models. It allows for the integration of various plugins to extend the capabilities of the AI assistant, making it more versatile and useful for different tasks.

## What is Function Calling?

Function Calling in the context of Semantic Kernel refers to the ability of the AI assistant to invoke specific functions or plugins based on the user's input. In the context of this project, this allows the AI assistant to perform tasks such as retrieving weather information, booking flights, finding hotels, and converting currencies, providing a more interactive and helpful experience for the user.

## Key Files

- [`appsettings.json`](SemanticKernelPluginsExample/appsettings.json): Contains the settings for connecting to the Ollama and Azure Open AI services.
- [`docker-compose.yaml`](docker-compose.yaml): Docker Compose configuration to set up the Ollama and Open WebUI services.
- [`docker/ollama/entrypoint.sh`](docker/ollama/entrypoint.sh): Script to start the Ollama service and pull the Llama model.
- [`SemanticKernelPluginsExample/Program.cs`](SemanticKernelPluginsExample/Program.cs): Main program file that sets up the Semantic Kernel and plugins.
- [`SemanticKernelPluginsExample/Plugins.cs`](SemanticKernelPluginsExample/Plugins.cs): Contains mock plugin implementations for retrieving weather, flights, hotels, and currency conversion rates which would be called by the AI.

## Prerequisites

You will need Microsoft .Net 9 SDK/runtime installed to build/run this project. If you intend to run the Ollama integration you will need the docker cli installed.

## Configuration

The configuration for the AI services is specified in the `appsettings.json` file. Here is an example configuration:

```json
{
    "aiServices": {
        "AzureOpenAi": {
            "modelId": "gpt-4o-mini",
            "url": "<your azure open ai endpoint url>",
            "apiKey": "<your api key>"
        },
        "Ollama": {
            "modelId": "llama3.2",
            "url": "http://localhost:11434",
            "timeoutSeconds": 300
        }
    }
}
```

### Setting Configuration for Azure OpenAI

To configure Azure OpenAI, update the `AzureOpenAi` section in the `appsettings.json` file with your model ID, URL, and API key.

### Setting Configuration for Ollama

To configure Ollama, update the `Ollama` section in the `appsettings.json` file with your model ID, URL, and timeout settings. You'd only need to do this if you changed the endpoint settings for ollama in the docker-compose file. If not, the default settings in the configuration file should just work.

## Usage

### Using the Local Ollama Service

- Start the ollama and open webui services using docker compose:

```sh
docker compose up
```

- Run the main program:

```sh
dotnet run --project SemanticKernelPluginsExample
```

or

```sh
dotnet run --project SemanticKernelPluginsExample -- --aiService Ollama
```

Note that it might take some time for the AI to respond the first time while the model is being loaded depending on the processing power and I/O performance of your machine. The docker-compose file allows for the use of NVidia CUDA cores if a supporting GPU is available.

You should also be able to access open-webui via [http://localhost:3001/](http://localhost:3001/) and directly interact with the locally running Llama model.

### Using Azure OpenAI

To use Azure OpenAI, run the program with the `AzureOpenAi` ai service option:

```sh
dotnet run --project SemanticKernelPluginsExample -- --aiService AzureOpenAi
```
