using Application.Common.Extensions;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Features.SalesReturnManager.Queries;

public record GetSalesReturnStatusListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
}

public class GetSalesReturnStatusListProfile : Profile
{
    public GetSalesReturnStatusListProfile()
    {
    }
}

public class GetSalesReturnStatusListResult
{
    public List<GetSalesReturnStatusListDto>? Data { get; init; }
}

public class GetSalesReturnStatusListRequest : IRequest<GetSalesReturnStatusListResult>
{
}


public class GetSalesReturnStatusListHandler : IRequestHandler<GetSalesReturnStatusListRequest, GetSalesReturnStatusListResult>
{

    public GetSalesReturnStatusListHandler()
    {
    }

    public async Task<GetSalesReturnStatusListResult> Handle(GetSalesReturnStatusListRequest request, CancellationToken cancellationToken)
    {
        var statuses = Enum.GetValues(typeof(SalesReturnStatus))
            .Cast<SalesReturnStatus>()
            .Select(status => new GetSalesReturnStatusListDto
            {
                Id = ((int)status).ToString(),
                Name = status.ToFriendlyName()
            })
            .ToList();

        await Task.CompletedTask;

        return new GetSalesReturnStatusListResult
        {
            Data = statuses
        };
    }


}



