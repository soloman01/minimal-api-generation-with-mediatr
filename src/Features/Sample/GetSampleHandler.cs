using MediatR;
using MinimalApiMediatorRegistration.Attributes;
using MinimalApiMediatorRegistration.Enums;

namespace MinimalApiMediatorRegistration.Features.Sample;

[Endpoint(HttpMethodType.Get, "Sample", "Get")]
public record GetSampleRequest() : IRequest<GetSampleResponse>;

public record GetSampleResponse(long Id, string Name);

public class GetSampleHandler : IRequestHandler<GetSampleRequest, GetSampleResponse>
{
    public Task<GetSampleResponse> Handle(GetSampleRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new GetSampleResponse(1,"Hello World"));
    }
}