using Application.Features.CustomerGroupManager.Commands;
using Application.Features.CustomerGroupManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class CustomerGroupController : BaseApiController
{
    public CustomerGroupController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateCustomerGroup")]
    public async Task<ActionResult<ApiSuccessResult<CreateCustomerGroupResult>>> CreateCustomerGroupAsync(CreateCustomerGroupRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateCustomerGroupResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateCustomerGroupAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateCustomerGroup")]
    public async Task<ActionResult<ApiSuccessResult<UpdateCustomerGroupResult>>> UpdateCustomerGroupAsync(UpdateCustomerGroupRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateCustomerGroupResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateCustomerGroupAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteCustomerGroup")]
    public async Task<ActionResult<ApiSuccessResult<DeleteCustomerGroupResult>>> DeleteCustomerGroupAsync(DeleteCustomerGroupRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteCustomerGroupResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteCustomerGroupAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetCustomerGroupList")]
    public async Task<ActionResult<ApiSuccessResult<GetCustomerGroupListResult>>> GetCustomerGroupListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetCustomerGroupListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetCustomerGroupListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetCustomerGroupListAsync)}",
            Content = response
        });
    }


}


