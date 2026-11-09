using Application.Common.Extensions;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Features.PurchaseOrderManager.Queries;

public record GetPurchaseOrderStatusListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
}

public class GetPurchaseOrderStatusListProfile : Profile
{
    public GetPurchaseOrderStatusListProfile()
    {
    }
}

public class GetPurchaseOrderStatusListResult
{
    public List<GetPurchaseOrderStatusListDto>? Data { get; init; }
}

public class GetPurchaseOrderStatusListRequest : IRequest<GetPurchaseOrderStatusListResult>
{
}


public class GetPurchaseOrderStatusListHandler : IRequestHandler<GetPurchaseOrderStatusListRequest, GetPurchaseOrderStatusListResult>
{

    public GetPurchaseOrderStatusListHandler()
    {
    }

    public async Task<GetPurchaseOrderStatusListResult> Handle(GetPurchaseOrderStatusListRequest request, CancellationToken cancellationToken)
    {
        var statuses = Enum.GetValues(typeof(PurchaseOrderStatus))
            .Cast<PurchaseOrderStatus>()
            .Select(status => new GetPurchaseOrderStatusListDto
            {
                Id = ((int)status).ToString(),
                Name = status.ToFriendlyName()
            })
            .ToList();

        await Task.CompletedTask;

        return new GetPurchaseOrderStatusListResult
        {
            Data = statuses
        };
    }


}



