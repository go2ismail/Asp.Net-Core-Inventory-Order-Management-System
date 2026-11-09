using Application.Common.Extensions;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Features.ScrappingManager.Queries;

public record GetScrappingStatusListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
}

public class GetScrappingStatusListProfile : Profile
{
    public GetScrappingStatusListProfile()
    {
    }
}

public class GetScrappingStatusListResult
{
    public List<GetScrappingStatusListDto>? Data { get; init; }
}

public class GetScrappingStatusListRequest : IRequest<GetScrappingStatusListResult>
{
}


public class GetScrappingStatusListHandler : IRequestHandler<GetScrappingStatusListRequest, GetScrappingStatusListResult>
{

    public GetScrappingStatusListHandler()
    {
    }

    public async Task<GetScrappingStatusListResult> Handle(GetScrappingStatusListRequest request, CancellationToken cancellationToken)
    {
        var statuses = Enum.GetValues(typeof(ScrappingStatus))
            .Cast<ScrappingStatus>()
            .Select(status => new GetScrappingStatusListDto
            {
                Id = ((int)status).ToString(),
                Name = status.ToFriendlyName()
            })
            .ToList();

        await Task.CompletedTask;

        return new GetScrappingStatusListResult
        {
            Data = statuses
        };
    }


}



