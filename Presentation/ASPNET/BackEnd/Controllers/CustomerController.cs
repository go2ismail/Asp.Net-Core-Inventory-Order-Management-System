using Application.Features.CustomerManager.Commands;
using Application.Features.CustomerManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class CustomerController : BaseApiController
{
    public CustomerController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateCustomer")]
    public async Task<ActionResult<ApiSuccessResult<CreateCustomerResult>>> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateCustomerResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateCustomerAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateCustomer")]
    public async Task<ActionResult<ApiSuccessResult<UpdateCustomerResult>>> UpdateCustomerAsync(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateCustomerResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateCustomerAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteCustomer")]
    public async Task<ActionResult<ApiSuccessResult<DeleteCustomerResult>>> DeleteCustomerAsync(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteCustomerResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteCustomerAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetCustomerList")]
    public async Task<ActionResult<ApiSuccessResult<GetCustomerListResult>>> GetCustomerListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetCustomerListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetCustomerListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetCustomerListAsync)}",
            Content = response
        });
    }


}


