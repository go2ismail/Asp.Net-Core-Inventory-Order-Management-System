using Application.Common.Extensions;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Features.PositiveAdjustmentManager.Queries;

public record GetPositiveAdjustmentStatusListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
}

public class GetPositiveAdjustmentStatusListProfile : Profile
{
    public GetPositiveAdjustmentStatusListProfile()
    {
    }
}

public class GetPositiveAdjustmentStatusListResult
{
    public List<GetPositiveAdjustmentStatusListDto>? Data { get; init; }
}

public class GetPositiveAdjustmentStatusListRequest : IRequest<GetPositiveAdjustmentStatusListResult>
{
}


public class GetPositiveAdjustmentStatusListHandler : IRequestHandler<GetPositiveAdjustmentStatusListRequest, GetPositiveAdjustmentStatusListResult>
{

    public GetPositiveAdjustmentStatusListHandler()
    {
    }

    public async Task<GetPositiveAdjustmentStatusListResult> Handle(GetPositiveAdjustmentStatusListRequest request, CancellationToken cancellationToken)
    {
        var statuses = Enum.GetValues(typeof(AdjustmentStatus))
            .Cast<AdjustmentStatus>()
            .Select(status => new GetPositiveAdjustmentStatusListDto
            {
                Id = ((int)status).ToString(),
                Name = status.ToFriendlyName()
            })
            .ToList();

        await Task.CompletedTask;

        return new GetPositiveAdjustmentStatusListResult
        {
            Data = statuses
        };
    }


}



