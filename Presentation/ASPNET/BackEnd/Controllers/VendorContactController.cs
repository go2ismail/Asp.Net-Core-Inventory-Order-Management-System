using Application.Features.VendorContactManager.Commands;
using Application.Features.VendorContactManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class VendorContactController : BaseApiController
{
    public VendorContactController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateVendorContact")]
    public async Task<ActionResult<ApiSuccessResult<CreateVendorContactResult>>> CreateVendorContactAsync(CreateVendorContactRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateVendorContactResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateVendorContactAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateVendorContact")]
    public async Task<ActionResult<ApiSuccessResult<UpdateVendorContactResult>>> UpdateVendorContactAsync(UpdateVendorContactRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateVendorContactResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateVendorContactAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteVendorContact")]
    public async Task<ActionResult<ApiSuccessResult<DeleteVendorContactResult>>> DeleteVendorContactAsync(DeleteVendorContactRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteVendorContactResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteVendorContactAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetVendorContactList")]
    public async Task<ActionResult<ApiSuccessResult<GetVendorContactListResult>>> GetVendorContactListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetVendorContactListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetVendorContactListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetVendorContactListAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetVendorContactByVendorIdList")]
    public async Task<ActionResult<ApiSuccessResult<GetVendorContactByVendorIdListResult>>> GetVendorContactByVendorIdListAsync(
    CancellationToken cancellationToken,
    [FromQuery] string vendorId
    )
    {
        var request = new GetVendorContactByVendorIdListRequest { VendorId = vendorId };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetVendorContactByVendorIdListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetVendorContactByVendorIdListAsync)}",
            Content = response
        });
    }
}


