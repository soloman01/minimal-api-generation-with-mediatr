using System.Reflection;

namespace MinimalApiMediatorRegistration.Descriptors;
public record HandlerDescriptor
{
    public Type RequestType { get; set; }
    private Attribute[] _requestTypeAttributes;
    public Attribute[] RequestTypeAttributes => _requestTypeAttributes ??= RequestType.GetCustomAttributes()?.ToArray() ?? [];
    public Type ResponseType { get; set; }
}
