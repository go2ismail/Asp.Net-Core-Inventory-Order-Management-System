using Application.Features.ProductGroupManager.Commands;
using Application.Features.ProductGroupManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class ProductGroupController : BaseApiController
{
    public ProductGroupController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateProductGroup")]
    public async Task<ActionResult<ApiSuccessResult<CreateProductGroupResult>>> CreateProductGroupAsync(CreateProductGroupRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateProductGroupResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateProductGroupAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateProductGroup")]
    public async Task<ActionResult<ApiSuccessResult<UpdateProductGroupResult>>> UpdateProductGroupAsync(UpdateProductGroupRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateProductGroupResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateProductGroupAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteProductGroup")]
    public async Task<ActionResult<ApiSuccessResult<DeleteProductGroupResult>>> DeleteProductGroupAsync(DeleteProductGroupRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteProductGroupResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteProductGroupAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetProductGroupList")]
    public async Task<ActionResult<ApiSuccessResult<GetProductGroupListResult>>> GetProductGroupListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetProductGroupListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetProductGroupListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetProductGroupListAsync)}",
            Content = response
        });
    }


}


