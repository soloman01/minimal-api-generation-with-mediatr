using MediatR;
using MinimalApiMediatorRegistration.Attributes;
using MinimalApiMediatorRegistration.Enums;

namespace MinimalApiMediatorRegistration.Features.Sample;

[Endpoint(HttpMethodType.Delete, "Sample", "Delete")]
public record DeleteSampleRequest(long Id) : IRequest<DeleteSampleResponse>;

public record DeleteSampleResponse(long Id);

public class DeleteSampleHandler : IRequestHandler<DeleteSampleRequest, DeleteSampleResponse>
{
    public Task<DeleteSampleResponse> Handle(DeleteSampleRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new DeleteSampleResponse(request.Id));
    }
}