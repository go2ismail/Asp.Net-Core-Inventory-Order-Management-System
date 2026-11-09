using Application.Features.SalesReturnManager.Commands;
using Application.Features.SalesReturnManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class SalesReturnController : BaseApiController
{
    public SalesReturnController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateSalesReturn")]
    public async Task<ActionResult<ApiSuccessResult<CreateSalesReturnResult>>> CreateSalesReturnAsync(CreateSalesReturnRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateSalesReturnResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateSalesReturnAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateSalesReturn")]
    public async Task<ActionResult<ApiSuccessResult<UpdateSalesReturnResult>>> UpdateSalesReturnAsync(UpdateSalesReturnRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateSalesReturnResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateSalesReturnAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteSalesReturn")]
    public async Task<ActionResult<ApiSuccessResult<DeleteSalesReturnResult>>> DeleteSalesReturnAsync(DeleteSalesReturnRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteSalesReturnResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteSalesReturnAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetSalesReturnList")]
    public async Task<ActionResult<ApiSuccessResult<GetSalesReturnListResult>>> GetSalesReturnListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetSalesReturnListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetSalesReturnListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetSalesReturnListAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetSalesReturnStatusList")]
    public async Task<ActionResult<ApiSuccessResult<GetSalesReturnStatusListResult>>> GetSalesReturnStatusListAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetSalesReturnStatusListRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetSalesReturnStatusListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetSalesReturnStatusListAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetSalesReturnSingle")]
    public async Task<ActionResult<ApiSuccessResult<GetSalesReturnSingleResult>>> GetSalesReturnSingleAsync(
    CancellationToken cancellationToken,
    [FromQuery] string id
    )
    {
        var request = new GetSalesReturnSingleRequest { Id = id };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetSalesReturnSingleResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetSalesReturnSingleAsync)}",
            Content = response
        });
    }


}


