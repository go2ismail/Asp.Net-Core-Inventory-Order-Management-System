using Application.Features.CustomerCategoryManager.Commands;
using Application.Features.CustomerCategoryManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class CustomerCategoryController : BaseApiController
{
    public CustomerCategoryController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateCustomerCategory")]
    public async Task<ActionResult<ApiSuccessResult<CreateCustomerCategoryResult>>> CreateCustomerCategoryAsync(CreateCustomerCategoryRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateCustomerCategoryResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateCustomerCategoryAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateCustomerCategory")]
    public async Task<ActionResult<ApiSuccessResult<UpdateCustomerCategoryResult>>> UpdateCustomerCategoryAsync(UpdateCustomerCategoryRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateCustomerCategoryResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateCustomerCategoryAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteCustomerCategory")]
    public async Task<ActionResult<ApiSuccessResult<DeleteCustomerCategoryResult>>> DeleteCustomerCategoryAsync(DeleteCustomerCategoryRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteCustomerCategoryResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteCustomerCategoryAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetCustomerCategoryList")]
    public async Task<ActionResult<ApiSuccessResult<GetCustomerCategoryListResult>>> GetCustomerCategoryListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetCustomerCategoryListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetCustomerCategoryListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetCustomerCategoryListAsync)}",
            Content = response
        });
    }


}


