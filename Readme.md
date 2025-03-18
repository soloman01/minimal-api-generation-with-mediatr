# Minimal API Generation with MediatR

This project demonstrates **.NET Core 9 Minimal API** with **MediatR**, automatically generating Minimal API endpoints for each MediatR handler. With this approach, you can streamline API development by reducing manual endpoint definitions while maintaining a clean **CQRS (Command Query Responsibility Segregation)** architecture.

## 📌 Features
- **Automatic API Generation** for MediatR handlers
- **Minimal API** support for a lightweight and efficient API structure
- **CQRS** pattern implementation using **MediatR**
- **Swagger** integration for API documentation


## 🚀 Setup and Usage

### 1️⃣ Clone the Repository
```sh
git clone https://github.com/soloman01/minimal-api-generation-with-mediatr.git
cd minimal-api-mediatr
```

### 2️⃣ Install Dependencies
```sh
dotnet restore
```

### 3️⃣ Run the Application
```sh
dotnet run
```
After starting the API, you can access the **Swagger UI** at:

```
http://localhost:5000/swagger
```

## ⚙️ MediatR Automatic API Generation
This project automatically registers all IRequestHandler<> implementations and dynamically presents them as Minimal API endpoints. 

```csharp
public static class WebApplicationExtensions
{
    public static WebApplication MapEndpointsForMediatR(this WebApplication app)
    {
        var descriptors = Descriptor.Instance.GetHandlerDescriptors
            .Where(f => f.RequestTypeAttributes.Any(x => x.GetType() == typeof(HttpMethodAttribute)));

        var api = app.MapGroup("api");

        foreach (var descriptor in descriptors)
        {
            var httpMethodAttribute = (HttpMethodAttribute)descriptor.RequestTypeAttributes.First(f => f.GetType() == typeof(HttpMethodAttribute));

            var pattern = $"v{httpMethodAttribute.Version}/{httpMethodAttribute.Tag}/{httpMethodAttribute.Route}";
            var name = $"{httpMethodAttribute.Tag}_{httpMethodAttribute.Route}_v{httpMethodAttribute.Version}";

            var delegateMethod = DelegateGenerator.CreateDelegate(descriptor, httpMethodAttribute);

            var builder = httpMethodAttribute.HttpMethod switch
            {
                HttpMethodType.Post => api.MapPost(pattern, delegateMethod),
                HttpMethodType.Delete => api.MapDelete(pattern, delegateMethod),
                HttpMethodType.Put => api.MapPut(pattern, delegateMethod),
                HttpMethodType.Patch => api.MapPatch(pattern,delegateMethod),
                HttpMethodType.Options => api.MapMethods(pattern, ["OPTIONS"],delegateMethod),
                HttpMethodType.Head => api.MapMethods(pattern, ["HEAD"],delegateMethod),
                _ => api.MapGet(pattern, delegateMethod)
            };

            builder
                .WithName(name)
                .WithTags(httpMethodAttribute.Tag);
        }

        return app;
    }
}

```

## 📌 Example Usage
### ➤ Defining a Command
```csharp
[Endpoint(HttpMethodType.Post, "Sample", "Create",version:1)]
public record CreateSampleRequest(string Name) : IRequest<CreateSampleResponse>;

public record CreateSampleResponse(string Name);

```

### ➤ Implementing a Command Handler
```csharp
public class CreateSampleHandler : IRequestHandler<CreateSampleRequest, CreateSampleResponse>
{
    public Task<CreateSampleResponse> Handle(CreateSampleRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new CreateSampleResponse(request.Name));
    }
}
```

### ➤ Endpoint Attribute Explanation

The `[Endpoint]` attribute is used to automatically register and expose your command or query as a Minimal API endpoint. Here's a breakdown of its parameters:

```csharp
[Endpoint(HttpMethodType.Post, "Sample", "Create", version: 1)]

```

| Parameter                  | Description                                                                                                                              |
|---------------------------|------------------------------------------------------------------------------------------------------------------------------------------|
| `HttpMethodType.Post`      | **HTTP Method Type**: Defines the HTTP verb (e.g., `GET`, `POST`, `PUT`, `DELETE`) for the endpoint. Here, it sets the request to `POST`.  |
| `"Sample"`                 | **Group Name**: Specifies the route prefix/grouping for the endpoint. Organizes the endpoint under `/api/v1/Sample/`.                     |
| `"Create"`                 | **Action Name**: Determines the specific action name for the endpoint. Becomes the last part of the URL path (`/Create`).                 |
| `version: 1`               | **API Version**: Sets the API versioning. The version appears in the URL as `/api/v1/`. Makes versioning easy and maintainable.            |



### ➤ Auto-Generated Endpoint
The `CreateSampleHandler` automatically generates the following Minimal API endpoint:
```http
POST /api/v1/Sample/Create
```

## 🛠 Development Tools
- **.NET Core 9**
- **MediatR**
- **Swagger UI**



