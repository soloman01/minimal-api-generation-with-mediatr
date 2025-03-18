using MinimalApiMediatorRegistration.Attributes;
using MinimalApiMediatorRegistration.Delegates;

namespace MinimalApiMediatorRegistration.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapEndpointsForMediatR(this WebApplication app)
    {
        var descriptors = Descriptor.Instance.GetHandlerDescriptors
            .Where(f => f.RequestTypeAttributes.Any(x => x.GetType() == typeof(EndpointAttribute)));

        var api = app.MapGroup("api");

        foreach (var descriptor in descriptors)
        {
            var endpointAttribute = (EndpointAttribute)descriptor.RequestTypeAttributes.First(f => f.GetType() == typeof(EndpointAttribute));

            var pattern = $"v{endpointAttribute.Version}/{endpointAttribute.Group}/{endpointAttribute.Route}";
            var name = $"{endpointAttribute.Group}_{endpointAttribute.Route}_v{endpointAttribute.Version}";

            var delegateMethod = DelegateGenerator.CreateDelegate(descriptor, endpointAttribute);

            var builder = endpointAttribute.HttpMethod switch
            {
                Enums.HttpMethodType.Post => api.MapPost(pattern, delegateMethod),
                Enums.HttpMethodType.Delete => api.MapDelete(pattern, delegateMethod),
                Enums.HttpMethodType.Put => api.MapPut(pattern, delegateMethod),
                Enums.HttpMethodType.Patch => api.MapPatch(pattern, delegateMethod),
                Enums.HttpMethodType.Options => api.MapMethods(pattern, ["OPTIONS"], delegateMethod),
                Enums.HttpMethodType.Head => api.MapMethods(pattern, ["HEAD"], delegateMethod),
                _ => api.MapGet(pattern, delegateMethod)
            };

            builder
                .WithName(name)
                .WithTags(endpointAttribute.Group);
        }

        return app;
    }
}
