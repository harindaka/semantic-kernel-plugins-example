
# Extending AI (a Locally Hosted Llama3.2 Model) with Semantic Kernel Functions

This project demonstrates the fundamentals of building your own AI agent / co-pilot with the use of a locally hosted Llama3.2 model in conjuction with Microsoft Semantic Kernel and plugins which can be used to extend the capabilities of AI by getting it to invoke custom code based on prompt engineering and user input / queries. Watch the video below to see it in action:

[![Watch the video](https://img.youtube.com/vi/HzMQPx6Xhzw/0.jpg)](https://www.youtube.com/watch?v=HzMQPx6Xhzw)

## What is Semantic Kernel?

Microsoft Semantic Kernel is a framework that enables developers to build AI applications using natural language processing (NLP) models. It allows for the integration of various plugins to extend the capabilities of the AI assistant, making it more versatile and useful for different tasks.

## What is Function Calling?

Function Calling in the context of Semantic Kernel refers to the ability of the AI assistant to invoke specific functions or plugins based on the user's input. In the context of this project, this allows the AI assistant to perform tasks such as retrieving weather information, booking flights, finding hotels, and converting currencies, providing a more interactive and helpful experience for the user.

## Key Files

- [`docker-compose.yaml`](docker-compose.yaml): Docker Compose configuration to set up the Ollama and Open WebUI services.
- [`docker/ollama/entrypoint.sh`](docker/ollama/entrypoint.sh): Script to start the Ollama service and pull the Llama model.
- [`SemanticKernalPluginsExample/Program.cs`](SemanticKernalPluginsExample/Program.cs): Main program file that sets up the Semantic Kernel and plugins.
- [`SemanticKernalPluginsExample/Plugins.cs`](SemanticKernalPluginsExample/Plugins.cs): Contains mock plugin implementations for retrieving weather, flights, hotels, and currency conversion rates which would be called by the AI.

## Prerequisites

You will need Docker and Microsoft .Net 9 SDK installed to run this project.

## Usage

1. Start the services using Docker Compose:

```sh
docker-compose up
```

1. Run the main program:

```sh
dotnet run --project SemanticKernalPluginsExample
```

1. You should now be able to interact with the AI Assistant via the console.

## Additional Considerations

Note that it might take some time for the AI to respond the first time while the model is being loaded depending on the processing power and I/O performance of your machine. The docker-compose file allows for the use of NVidia CUDA cores if a supporting GPU is available.

You should also be able to access open-webui via [http://localhost:3001/](http://localhost:3001/) and directly interact with the locally running Llama model.
