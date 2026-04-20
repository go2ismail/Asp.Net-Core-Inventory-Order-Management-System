using Application.Features.BillOfMaterialManager.Commands;
using Application.Features.BillOfMaterialManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class BillOfMaterialController : BaseApiController
{
    public BillOfMaterialController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateBillOfMaterial")]
    public async Task<ActionResult<ApiSuccessResult<CreateBillOfMaterialResult>>> CreateBillOfMaterialAsync(CreateBillOfMaterialRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);
        return Ok(new ApiSuccessResult<CreateBillOfMaterialResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateBillOfMaterialAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateBillOfMaterial")]
    public async Task<ActionResult<ApiSuccessResult<UpdateBillOfMaterialResult>>> UpdateBillOfMaterialAsync(UpdateBillOfMaterialRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);
        return Ok(new ApiSuccessResult<UpdateBillOfMaterialResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateBillOfMaterialAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteBillOfMaterial")]
    public async Task<ActionResult<ApiSuccessResult<DeleteBillOfMaterialResult>>> DeleteBillOfMaterialAsync(DeleteBillOfMaterialRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);
        return Ok(new ApiSuccessResult<DeleteBillOfMaterialResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteBillOfMaterialAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetBillOfMaterialList")]
    public async Task<ActionResult<ApiSuccessResult<GetBillOfMaterialListResult>>> GetBillOfMaterialListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false,
        [FromQuery] string? productId = null,
        [FromQuery] bool? isActive = null)
    {
        var request = new GetBillOfMaterialListRequest
        {
            IsDeleted = isDeleted,
            ProductId = productId,
            IsActive = isActive
        };
        var response = await _sender.Send(request, cancellationToken);
        return Ok(new ApiSuccessResult<GetBillOfMaterialListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetBillOfMaterialListAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetBillOfMaterialByProduct")]
    public async Task<ActionResult<ApiSuccessResult<GetBillOfMaterialByProductResult>>> GetBillOfMaterialByProductAsync(
        CancellationToken cancellationToken,
        [FromQuery] string? productId = null,
        [FromQuery] bool getActiveOnly = true)
    {
        var request = new GetBillOfMaterialByProductRequest
        {
            ProductId = productId,
            GetActiveOnly = getActiveOnly
        };
        var response = await _sender.Send(request, cancellationToken);
        return Ok(new ApiSuccessResult<GetBillOfMaterialByProductResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetBillOfMaterialByProductAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetBillOfMaterialExplosion")]
    public async Task<ActionResult<ApiSuccessResult<GetBillOfMaterialExplosionResult>>> GetBillOfMaterialExplosionAsync(
        CancellationToken cancellationToken,
        [FromQuery] string? productId = null,
        [FromQuery] double quantity = 1)
    {
        var request = new GetBillOfMaterialExplosionRequest
        {
            ProductId = productId,
            Quantity = quantity
        };
        var response = await _sender.Send(request, cancellationToken);
        return Ok(new ApiSuccessResult<GetBillOfMaterialExplosionResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetBillOfMaterialExplosionAsync)}",
            Content = response
        });
    }
}
