using Application.Common.Extensions;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Features.GoodsReceiveManager.Queries;

public record GetGoodsReceiveStatusListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
}

public class GetGoodsReceiveStatusListProfile : Profile
{
    public GetGoodsReceiveStatusListProfile()
    {
    }
}

public class GetGoodsReceiveStatusListResult
{
    public List<GetGoodsReceiveStatusListDto>? Data { get; init; }
}

public class GetGoodsReceiveStatusListRequest : IRequest<GetGoodsReceiveStatusListResult>
{
}


public class GetGoodsReceiveStatusListHandler : IRequestHandler<GetGoodsReceiveStatusListRequest, GetGoodsReceiveStatusListResult>
{

    public GetGoodsReceiveStatusListHandler()
    {
    }

    public async Task<GetGoodsReceiveStatusListResult> Handle(GetGoodsReceiveStatusListRequest request, CancellationToken cancellationToken)
    {
        var statuses = Enum.GetValues(typeof(GoodsReceiveStatus))
            .Cast<GoodsReceiveStatus>()
            .Select(status => new GetGoodsReceiveStatusListDto
            {
                Id = ((int)status).ToString(),
                Name = status.ToFriendlyName()
            })
            .ToList();

        await Task.CompletedTask;

        return new GetGoodsReceiveStatusListResult
        {
            Data = statuses
        };
    }


}



