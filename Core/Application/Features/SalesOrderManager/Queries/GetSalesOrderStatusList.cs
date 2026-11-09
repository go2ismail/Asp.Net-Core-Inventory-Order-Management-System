using Application.Common.Extensions;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Features.SalesOrderManager.Queries;

public record GetSalesOrderStatusListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
}

public class GetSalesOrderStatusListProfile : Profile
{
    public GetSalesOrderStatusListProfile()
    {
    }
}

public class GetSalesOrderStatusListResult
{
    public List<GetSalesOrderStatusListDto>? Data { get; init; }
}

public class GetSalesOrderStatusListRequest : IRequest<GetSalesOrderStatusListResult>
{
}


public class GetSalesOrderStatusListHandler : IRequestHandler<GetSalesOrderStatusListRequest, GetSalesOrderStatusListResult>
{

    public GetSalesOrderStatusListHandler()
    {
    }

    public async Task<GetSalesOrderStatusListResult> Handle(GetSalesOrderStatusListRequest request, CancellationToken cancellationToken)
    {
        var statuses = Enum.GetValues(typeof(SalesOrderStatus))
            .Cast<SalesOrderStatus>()
            .Select(status => new GetSalesOrderStatusListDto
            {
                Id = ((int)status).ToString(),
                Name = status.ToFriendlyName()
            })
            .ToList();

        await Task.CompletedTask;

        return new GetSalesOrderStatusListResult
        {
            Data = statuses
        };
    }


}



