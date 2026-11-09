using Application.Common.Extensions;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Features.NegativeAdjustmentManager.Queries;

public record GetNegativeAdjustmentStatusListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
}

public class GetNegativeAdjustmentStatusListProfile : Profile
{
    public GetNegativeAdjustmentStatusListProfile()
    {
    }
}

public class GetNegativeAdjustmentStatusListResult
{
    public List<GetNegativeAdjustmentStatusListDto>? Data { get; init; }
}

public class GetNegativeAdjustmentStatusListRequest : IRequest<GetNegativeAdjustmentStatusListResult>
{
}


public class GetNegativeAdjustmentStatusListHandler : IRequestHandler<GetNegativeAdjustmentStatusListRequest, GetNegativeAdjustmentStatusListResult>
{

    public GetNegativeAdjustmentStatusListHandler()
    {
    }

    public async Task<GetNegativeAdjustmentStatusListResult> Handle(GetNegativeAdjustmentStatusListRequest request, CancellationToken cancellationToken)
    {
        var statuses = Enum.GetValues(typeof(AdjustmentStatus))
            .Cast<AdjustmentStatus>()
            .Select(status => new GetNegativeAdjustmentStatusListDto
            {
                Id = ((int)status).ToString(),
                Name = status.ToFriendlyName()
            })
            .ToList();

        await Task.CompletedTask;

        return new GetNegativeAdjustmentStatusListResult
        {
            Data = statuses
        };
    }


}



