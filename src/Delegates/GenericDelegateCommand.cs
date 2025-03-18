using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using MinimalApiMediatorRegistration.Abstractions;

namespace MinimalApiMediatorRegistration.Delegates;
internal class GenericDelegateCommand<TRequest, TResponse> : IGenericDelegate where TRequest : IRequest<TResponse>
{
    public Delegate Func => async (ISender sender, TRequest request, CancellationToken cancellationToken) =>
    {
        Results<Ok<TResponse>, BadRequest> httpResult;

        try
        {
            var result = await sender.Send(request, cancellationToken);
            return TypedResults.Ok(result);
        }
        catch
        {
            httpResult = TypedResults.BadRequest();
        }

        return httpResult;
    };
}