using Application.Features.CustomerContactManager.Commands;
using Application.Features.CustomerContactManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class CustomerContactController : BaseApiController
{
    public CustomerContactController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateCustomerContact")]
    public async Task<ActionResult<ApiSuccessResult<CreateCustomerContactResult>>> CreateCustomerContactAsync(CreateCustomerContactRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateCustomerContactResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateCustomerContactAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateCustomerContact")]
    public async Task<ActionResult<ApiSuccessResult<UpdateCustomerContactResult>>> UpdateCustomerContactAsync(UpdateCustomerContactRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateCustomerContactResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateCustomerContactAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteCustomerContact")]
    public async Task<ActionResult<ApiSuccessResult<DeleteCustomerContactResult>>> DeleteCustomerContactAsync(DeleteCustomerContactRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteCustomerContactResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteCustomerContactAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetCustomerContactList")]
    public async Task<ActionResult<ApiSuccessResult<GetCustomerContactListResult>>> GetCustomerContactListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetCustomerContactListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetCustomerContactListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetCustomerContactListAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetCustomerContactByCustomerIdList")]
    public async Task<ActionResult<ApiSuccessResult<GetCustomerContactByCustomerIdListResult>>> GetCustomerContactByCustomerIdListAsync(
    CancellationToken cancellationToken,
    [FromQuery] string customerId
    )
    {
        var request = new GetCustomerContactByCustomerIdListRequest { CustomerId = customerId };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetCustomerContactByCustomerIdListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetCustomerContactByCustomerIdListAsync)}",
            Content = response
        });
    }


}


