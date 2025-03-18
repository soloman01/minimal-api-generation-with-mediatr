using MediatR;
using MinimalApiMediatorRegistration.Attributes;
using MinimalApiMediatorRegistration.Enums;

namespace MinimalApiMediatorRegistration.Features.Sample;

[Endpoint(HttpMethodType.Put, "Sample", "Update")]
public record UpdateSampleRequest(long Id ,string Name) : IRequest<UpdateSampleResponse>;

public record UpdateSampleResponse(long Id, string Name);

public class UpdateSampleHandler : IRequestHandler<UpdateSampleRequest, UpdateSampleResponse>
{
    public Task<UpdateSampleResponse> Handle(UpdateSampleRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new UpdateSampleResponse(request.Id, request.Name));
    }
}