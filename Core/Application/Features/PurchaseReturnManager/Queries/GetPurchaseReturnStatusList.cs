using Application.Common.Extensions;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Features.PurchaseReturnManager.Queries;

public record GetPurchaseReturnStatusListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
}

public class GetPurchaseReturnStatusListProfile : Profile
{
    public GetPurchaseReturnStatusListProfile()
    {
    }
}

public class GetPurchaseReturnStatusListResult
{
    public List<GetPurchaseReturnStatusListDto>? Data { get; init; }
}

public class GetPurchaseReturnStatusListRequest : IRequest<GetPurchaseReturnStatusListResult>
{
}


public class GetPurchaseReturnStatusListHandler : IRequestHandler<GetPurchaseReturnStatusListRequest, GetPurchaseReturnStatusListResult>
{

    public GetPurchaseReturnStatusListHandler()
    {
    }

    public async Task<GetPurchaseReturnStatusListResult> Handle(GetPurchaseReturnStatusListRequest request, CancellationToken cancellationToken)
    {
        var statuses = Enum.GetValues(typeof(PurchaseReturnStatus))
            .Cast<PurchaseReturnStatus>()
            .Select(status => new GetPurchaseReturnStatusListDto
            {
                Id = ((int)status).ToString(),
                Name = status.ToFriendlyName()
            })
            .ToList();

        await Task.CompletedTask;

        return new GetPurchaseReturnStatusListResult
        {
            Data = statuses
        };
    }


}



