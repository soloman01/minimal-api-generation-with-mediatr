using MediatR;
using MinimalApiMediatorRegistration.Attributes;
using MinimalApiMediatorRegistration.Enums;

namespace MinimalApiMediatorRegistration.Features.Sample;

[Endpoint(HttpMethodType.Post, "Sample", "Create",version:1)]
public record CreateSampleRequest(string Name) : IRequest<CreateSampleResponse>;

public record CreateSampleResponse(string Name);

public class CreateSampleHandler : IRequestHandler<CreateSampleRequest, CreateSampleResponse>
{
    public Task<CreateSampleResponse> Handle(CreateSampleRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new CreateSampleResponse(request.Name));
    }
}