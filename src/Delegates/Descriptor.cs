using MediatR;
using MinimalApiMediatorRegistration.Descriptors;
using MinimalApiMediatorRegistration.Extensions;

namespace MinimalApiMediatorRegistration.Delegates;

public class Descriptor
{
    private static readonly Lazy<Descriptor> _instance = new(() => new Descriptor());
    public static Descriptor Instance => _instance.Value;


    private HandlerDescriptor[] _handlerDescriptors;
    public IEnumerable<HandlerDescriptor> GetHandlerDescriptors => _handlerDescriptors;

    private Descriptor()
    {
        _handlerDescriptors = [.. EnumerableHandlerDescriptor(typeof(IRequestHandler<,>))];
    }

    private static IEnumerable<HandlerDescriptor> EnumerableHandlerDescriptor(params Type[] fromInterfaces)
        => AppDomain.CurrentDomain
            .EnumerableTypes(f => f.IsClass, fromInterfaces)
            .Select(f =>
            {
                Type[] genericArguments = f.Interface!.GetGenericArguments()!;
                var requestType = genericArguments[0];

                return new HandlerDescriptor
                {
                    RequestType = requestType,
                    ResponseType = genericArguments.Length != 0 ? genericArguments[1] : null
                };
            });
}

