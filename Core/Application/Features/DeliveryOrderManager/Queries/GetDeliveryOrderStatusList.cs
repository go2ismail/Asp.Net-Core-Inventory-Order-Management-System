using Application.Common.Extensions;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Features.DeliveryOrderManager.Queries;

public record GetDeliveryOrderStatusListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
}

public class GetDeliveryOrderStatusListProfile : Profile
{
    public GetDeliveryOrderStatusListProfile()
    {
    }
}

public class GetDeliveryOrderStatusListResult
{
    public List<GetDeliveryOrderStatusListDto>? Data { get; init; }
}

public class GetDeliveryOrderStatusListRequest : IRequest<GetDeliveryOrderStatusListResult>
{
}


public class GetDeliveryOrderStatusListHandler : IRequestHandler<GetDeliveryOrderStatusListRequest, GetDeliveryOrderStatusListResult>
{

    public GetDeliveryOrderStatusListHandler()
    {
    }

    public async Task<GetDeliveryOrderStatusListResult> Handle(GetDeliveryOrderStatusListRequest request, CancellationToken cancellationToken)
    {
        var statuses = Enum.GetValues(typeof(DeliveryOrderStatus))
            .Cast<DeliveryOrderStatus>()
            .Select(status => new GetDeliveryOrderStatusListDto
            {
                Id = ((int)status).ToString(),
                Name = status.ToFriendlyName()
            })
            .ToList();

        await Task.CompletedTask;

        return new GetDeliveryOrderStatusListResult
        {
            Data = statuses
        };
    }


}



