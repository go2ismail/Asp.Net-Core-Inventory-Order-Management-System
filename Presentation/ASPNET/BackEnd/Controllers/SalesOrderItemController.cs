using Application.Features.SalesOrderItemManager.Commands;
using Application.Features.SalesOrderItemManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class SalesOrderItemController : BaseApiController
{
    public SalesOrderItemController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateSalesOrderItem")]
    public async Task<ActionResult<ApiSuccessResult<CreateSalesOrderItemResult>>> CreateSalesOrderItemAsync(CreateSalesOrderItemRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateSalesOrderItemResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateSalesOrderItemAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateSalesOrderItem")]
    public async Task<ActionResult<ApiSuccessResult<UpdateSalesOrderItemResult>>> UpdateSalesOrderItemAsync(UpdateSalesOrderItemRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateSalesOrderItemResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateSalesOrderItemAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteSalesOrderItem")]
    public async Task<ActionResult<ApiSuccessResult<DeleteSalesOrderItemResult>>> DeleteSalesOrderItemAsync(DeleteSalesOrderItemRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteSalesOrderItemResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteSalesOrderItemAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetSalesOrderItemList")]
    public async Task<ActionResult<ApiSuccessResult<GetSalesOrderItemListResult>>> GetSalesOrderItemListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetSalesOrderItemListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetSalesOrderItemListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetSalesOrderItemListAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetSalesOrderItemBySalesOrderIdList")]
    public async Task<ActionResult<ApiSuccessResult<GetSalesOrderItemBySalesOrderIdListResult>>> GetSalesOrderItemBySalesOrderIdListAsync(
    CancellationToken cancellationToken,
    [FromQuery] string salesOrderId
    )
    {
        var request = new GetSalesOrderItemBySalesOrderIdListRequest { SalesOrderId = salesOrderId };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetSalesOrderItemBySalesOrderIdListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetSalesOrderItemBySalesOrderIdListAsync)}",
            Content = response
        });
    }


}


