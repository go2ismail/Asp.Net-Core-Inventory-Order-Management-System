using Application.Features.BillOfMaterialItemManager.Commands;
using Application.Features.BillOfMaterialItemManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class BillOfMaterialItemController : BaseApiController
{
    public BillOfMaterialItemController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateBillOfMaterialItem")]
    public async Task<ActionResult<ApiSuccessResult<CreateBillOfMaterialItemResult>>> CreateBillOfMaterialItemAsync(CreateBillOfMaterialItemRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);
        return Ok(new ApiSuccessResult<CreateBillOfMaterialItemResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateBillOfMaterialItemAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateBillOfMaterialItem")]
    public async Task<ActionResult<ApiSuccessResult<UpdateBillOfMaterialItemResult>>> UpdateBillOfMaterialItemAsync(UpdateBillOfMaterialItemRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);
        return Ok(new ApiSuccessResult<UpdateBillOfMaterialItemResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateBillOfMaterialItemAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteBillOfMaterialItem")]
    public async Task<ActionResult<ApiSuccessResult<DeleteBillOfMaterialItemResult>>> DeleteBillOfMaterialItemAsync(DeleteBillOfMaterialItemRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);
        return Ok(new ApiSuccessResult<DeleteBillOfMaterialItemResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteBillOfMaterialItemAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetBillOfMaterialItemList")]
    public async Task<ActionResult<ApiSuccessResult<GetBillOfMaterialItemListResult>>> GetBillOfMaterialItemListAsync(
        CancellationToken cancellationToken,
        [FromQuery] string? billOfMaterialId = null)
    {
        var request = new GetBillOfMaterialItemListRequest
        {
            BillOfMaterialId = billOfMaterialId
        };
        var response = await _sender.Send(request, cancellationToken);
        return Ok(new ApiSuccessResult<GetBillOfMaterialItemListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetBillOfMaterialItemListAsync)}",
            Content = response
        });
    }
}
