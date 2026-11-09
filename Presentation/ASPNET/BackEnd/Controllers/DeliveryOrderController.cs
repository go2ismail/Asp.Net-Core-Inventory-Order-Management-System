using Application.Features.DeliveryOrderManager.Commands;
using Application.Features.DeliveryOrderManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class DeliveryOrderController : BaseApiController
{
    public DeliveryOrderController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateDeliveryOrder")]
    public async Task<ActionResult<ApiSuccessResult<CreateDeliveryOrderResult>>> CreateDeliveryOrderAsync(CreateDeliveryOrderRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateDeliveryOrderResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateDeliveryOrderAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateDeliveryOrder")]
    public async Task<ActionResult<ApiSuccessResult<UpdateDeliveryOrderResult>>> UpdateDeliveryOrderAsync(UpdateDeliveryOrderRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateDeliveryOrderResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateDeliveryOrderAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteDeliveryOrder")]
    public async Task<ActionResult<ApiSuccessResult<DeleteDeliveryOrderResult>>> DeleteDeliveryOrderAsync(DeleteDeliveryOrderRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteDeliveryOrderResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteDeliveryOrderAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetDeliveryOrderList")]
    public async Task<ActionResult<ApiSuccessResult<GetDeliveryOrderListResult>>> GetDeliveryOrderListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetDeliveryOrderListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetDeliveryOrderListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetDeliveryOrderListAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetDeliveryOrderStatusList")]
    public async Task<ActionResult<ApiSuccessResult<GetDeliveryOrderStatusListResult>>> GetDeliveryOrderStatusListAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetDeliveryOrderStatusListRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetDeliveryOrderStatusListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetDeliveryOrderStatusListAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetDeliveryOrderSingle")]
    public async Task<ActionResult<ApiSuccessResult<GetDeliveryOrderSingleResult>>> GetDeliveryOrderSingleAsync(
    CancellationToken cancellationToken,
    [FromQuery] string id
    )
    {
        var request = new GetDeliveryOrderSingleRequest { Id = id };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetDeliveryOrderSingleResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetDeliveryOrderSingleAsync)}",
            Content = response
        });
    }


}


