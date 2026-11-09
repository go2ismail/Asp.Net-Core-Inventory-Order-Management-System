using Application.Features.VendorGroupManager.Commands;
using Application.Features.VendorGroupManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class VendorGroupController : BaseApiController
{
    public VendorGroupController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateVendorGroup")]
    public async Task<ActionResult<ApiSuccessResult<CreateVendorGroupResult>>> CreateVendorGroupAsync(CreateVendorGroupRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateVendorGroupResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateVendorGroupAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateVendorGroup")]
    public async Task<ActionResult<ApiSuccessResult<UpdateVendorGroupResult>>> UpdateVendorGroupAsync(UpdateVendorGroupRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateVendorGroupResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateVendorGroupAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteVendorGroup")]
    public async Task<ActionResult<ApiSuccessResult<DeleteVendorGroupResult>>> DeleteVendorGroupAsync(DeleteVendorGroupRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteVendorGroupResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteVendorGroupAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetVendorGroupList")]
    public async Task<ActionResult<ApiSuccessResult<GetVendorGroupListResult>>> GetVendorGroupListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetVendorGroupListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetVendorGroupListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetVendorGroupListAsync)}",
            Content = response
        });
    }


}


