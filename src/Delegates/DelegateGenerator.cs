using MinimalApiMediatorRegistration.Abstractions;
using MinimalApiMediatorRegistration.Attributes;
using MinimalApiMediatorRegistration.Descriptors;
using MinimalApiMediatorRegistration.Enums;

namespace MinimalApiMediatorRegistration.Delegates;

public static class DelegateGenerator
{
    public static Delegate CreateDelegate(HandlerDescriptor descriptor, EndpointAttribute endpointAttribute)
    {
        var responseType = descriptor.ResponseType;

        var genericType = endpointAttribute.HttpMethod switch
        {
            HttpMethodType.Get or HttpMethodType.Delete or HttpMethodType.Options or HttpMethodType.Head
                => typeof(GenericDelegateQuery<,>),

            HttpMethodType.Post or HttpMethodType.Put or HttpMethodType.Patch
                => typeof(GenericDelegateCommand<,>),
            _ => throw new NotSupportedException($"HTTP method {endpointAttribute.HttpMethod} is not supported.")
        };

        var type = genericType.MakeGenericType(descriptor.RequestType, responseType);

        return ((IGenericDelegate)Activator.CreateInstance(type)!).Func;
    }

}
