namespace MinimalApiMediatorRegistration.Extensions;

public static class TypeExtensions
{
    public static Type FindInterface(this Type type, params Type[] from)
    {
        return type.GetInterfaces().FirstOrDefault(i => from.Contains(i) || (i.IsGenericType && from.Contains(i.GetGenericTypeDefinition())));
    }

    public static IEnumerable<(Type Type, Type Interface)> EnumerableTypes(this AppDomain domain, Func<Type, bool> typeFilterPredicate, params Type[] fromInterfaces)
    {
        return domain
            .GetAssemblies()
            .SelectMany(f => f.GetTypes())
            .Where(f =>
            {
                if (typeFilterPredicate is null)
                    return true;

                return typeFilterPredicate(f);
            })
            .Select(f => (
                Type: f,
                Interface: f.FindInterface(fromInterfaces)
            ))
            .Where(f => f.Interface != null);
    }
}
