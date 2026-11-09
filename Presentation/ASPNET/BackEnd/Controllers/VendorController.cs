using Application.Features.VendorManager.Commands;
using Application.Features.VendorManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class VendorController : BaseApiController
{
    public VendorController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateVendor")]
    public async Task<ActionResult<ApiSuccessResult<CreateVendorResult>>> CreateVendorAsync(CreateVendorRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateVendorResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateVendorAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateVendor")]
    public async Task<ActionResult<ApiSuccessResult<UpdateVendorResult>>> UpdateVendorAsync(UpdateVendorRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateVendorResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateVendorAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteVendor")]
    public async Task<ActionResult<ApiSuccessResult<DeleteVendorResult>>> DeleteVendorAsync(DeleteVendorRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteVendorResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteVendorAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetVendorList")]
    public async Task<ActionResult<ApiSuccessResult<GetVendorListResult>>> GetVendorListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetVendorListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetVendorListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetVendorListAsync)}",
            Content = response
        });
    }


}


