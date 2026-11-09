using Application.Common.Extensions;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Features.StockCountManager.Queries;

public record GetStockCountStatusListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
}

public class GetStockCountStatusListProfile : Profile
{
    public GetStockCountStatusListProfile()
    {
    }
}

public class GetStockCountStatusListResult
{
    public List<GetStockCountStatusListDto>? Data { get; init; }
}

public class GetStockCountStatusListRequest : IRequest<GetStockCountStatusListResult>
{
}


public class GetStockCountStatusListHandler : IRequestHandler<GetStockCountStatusListRequest, GetStockCountStatusListResult>
{

    public GetStockCountStatusListHandler()
    {
    }

    public async Task<GetStockCountStatusListResult> Handle(GetStockCountStatusListRequest request, CancellationToken cancellationToken)
    {
        var statuses = Enum.GetValues(typeof(StockCountStatus))
            .Cast<StockCountStatus>()
            .Select(status => new GetStockCountStatusListDto
            {
                Id = ((int)status).ToString(),
                Name = status.ToFriendlyName()
            })
            .ToList();

        await Task.CompletedTask;

        return new GetStockCountStatusListResult
        {
            Data = statuses
        };
    }


}



