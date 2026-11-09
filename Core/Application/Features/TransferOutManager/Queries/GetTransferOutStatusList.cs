using Application.Common.Extensions;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Features.TransferOutManager.Queries;

public record GetTransferOutStatusListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
}

public class GetTransferOutStatusListProfile : Profile
{
    public GetTransferOutStatusListProfile()
    {
    }
}

public class GetTransferOutStatusListResult
{
    public List<GetTransferOutStatusListDto>? Data { get; init; }
}

public class GetTransferOutStatusListRequest : IRequest<GetTransferOutStatusListResult>
{
}


public class GetTransferOutStatusListHandler : IRequestHandler<GetTransferOutStatusListRequest, GetTransferOutStatusListResult>
{

    public GetTransferOutStatusListHandler()
    {
    }

    public async Task<GetTransferOutStatusListResult> Handle(GetTransferOutStatusListRequest request, CancellationToken cancellationToken)
    {
        var statuses = Enum.GetValues(typeof(TransferStatus))
            .Cast<TransferStatus>()
            .Select(status => new GetTransferOutStatusListDto
            {
                Id = ((int)status).ToString(),
                Name = status.ToFriendlyName()
            })
            .ToList();

        await Task.CompletedTask;

        return new GetTransferOutStatusListResult
        {
            Data = statuses
        };
    }


}



