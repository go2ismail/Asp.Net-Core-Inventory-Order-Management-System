using Application.Features.VendorCategoryManager.Commands;
using Application.Features.VendorCategoryManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class VendorCategoryController : BaseApiController
{
    public VendorCategoryController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateVendorCategory")]
    public async Task<ActionResult<ApiSuccessResult<CreateVendorCategoryResult>>> CreateVendorCategoryAsync(CreateVendorCategoryRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateVendorCategoryResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateVendorCategoryAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateVendorCategory")]
    public async Task<ActionResult<ApiSuccessResult<UpdateVendorCategoryResult>>> UpdateVendorCategoryAsync(UpdateVendorCategoryRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateVendorCategoryResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateVendorCategoryAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteVendorCategory")]
    public async Task<ActionResult<ApiSuccessResult<DeleteVendorCategoryResult>>> DeleteVendorCategoryAsync(DeleteVendorCategoryRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteVendorCategoryResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteVendorCategoryAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetVendorCategoryList")]
    public async Task<ActionResult<ApiSuccessResult<GetVendorCategoryListResult>>> GetVendorCategoryListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetVendorCategoryListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetVendorCategoryListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetVendorCategoryListAsync)}",
            Content = response
        });
    }


}


