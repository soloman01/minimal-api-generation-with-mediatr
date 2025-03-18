using MinimalApiMediatorRegistration.Enums;

namespace MinimalApiMediatorRegistration.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class EndpointAttribute : Attribute
{
    public HttpMethodType HttpMethod { get; private set; }
    public string Group { get; private set; }
    public string Route { get; private set; }
    public uint Version { get; private set; }
    public EndpointAttribute(HttpMethodType httpMethod, string group, string route, uint version = 1)
    {
        HttpMethod = httpMethod;
        Group = group;
        Route = route;
        Version = version;
    }
}

