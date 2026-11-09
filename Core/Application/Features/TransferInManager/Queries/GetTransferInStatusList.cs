using Application.Common.Extensions;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Features.TransferInManager.Queries;

public record GetTransferInStatusListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
}

public class GetTransferInStatusListProfile : Profile
{
    public GetTransferInStatusListProfile()
    {
    }
}

public class GetTransferInStatusListResult
{
    public List<GetTransferInStatusListDto>? Data { get; init; }
}

public class GetTransferInStatusListRequest : IRequest<GetTransferInStatusListResult>
{
}


public class GetTransferInStatusListHandler : IRequestHandler<GetTransferInStatusListRequest, GetTransferInStatusListResult>
{

    public GetTransferInStatusListHandler()
    {
    }

    public async Task<GetTransferInStatusListResult> Handle(GetTransferInStatusListRequest request, CancellationToken cancellationToken)
    {
        var statuses = Enum.GetValues(typeof(TransferStatus))
            .Cast<TransferStatus>()
            .Select(status => new GetTransferInStatusListDto
            {
                Id = ((int)status).ToString(),
                Name = status.ToFriendlyName()
            })
            .ToList();

        await Task.CompletedTask;

        return new GetTransferInStatusListResult
        {
            Data = statuses
        };

    }


}



