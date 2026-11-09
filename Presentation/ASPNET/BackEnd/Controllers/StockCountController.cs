using Application.Features.StockCountManager.Commands;
using Application.Features.StockCountManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class StockCountController : BaseApiController
{
    public StockCountController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateStockCount")]
    public async Task<ActionResult<ApiSuccessResult<CreateStockCountResult>>> CreateStockCountAsync(CreateStockCountRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateStockCountResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateStockCountAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateStockCount")]
    public async Task<ActionResult<ApiSuccessResult<UpdateStockCountResult>>> UpdateStockCountAsync(UpdateStockCountRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateStockCountResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateStockCountAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteStockCount")]
    public async Task<ActionResult<ApiSuccessResult<DeleteStockCountResult>>> DeleteStockCountAsync(DeleteStockCountRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteStockCountResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteStockCountAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetStockCountList")]
    public async Task<ActionResult<ApiSuccessResult<GetStockCountListResult>>> GetStockCountListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetStockCountListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetStockCountListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetStockCountListAsync)}",
            Content = response
        });
    }




    [Authorize]
    [HttpGet("GetStockCountStatusList")]
    public async Task<ActionResult<ApiSuccessResult<GetStockCountStatusListResult>>> GetStockCountStatusListAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetStockCountStatusListRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetStockCountStatusListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetStockCountStatusListAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetStockCountSingle")]
    public async Task<ActionResult<ApiSuccessResult<GetStockCountSingleResult>>> GetStockCountSingleAsync(
    CancellationToken cancellationToken,
    [FromQuery] string id
    )
    {
        var request = new GetStockCountSingleRequest { Id = id };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetStockCountSingleResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetStockCountSingleAsync)}",
            Content = response
        });
    }
}


