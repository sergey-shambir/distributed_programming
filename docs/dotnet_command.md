# .NET core hints

Create new project:

```bash
dotnet new mvc --name "Project"
dotnet new console --name "Project"
dotnet new classlib --name "Project"
```

Add package:

```bash
dotnet add package Newtonsoft.Json --version 11.0.2
dotnet add package StackExchange.Redis --version 1.2.6
dotnet add package RabbitMQ.Client --version 5.0.1
```

Build or run project:

```bash
dotnet build --project Backend
dotnet run --project Backend
```
