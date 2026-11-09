using Application.Features.PositiveAdjustmentManager.Commands;
using Application.Features.PositiveAdjustmentManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class PositiveAdjustmentController : BaseApiController
{
    public PositiveAdjustmentController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreatePositiveAdjustment")]
    public async Task<ActionResult<ApiSuccessResult<CreatePositiveAdjustmentResult>>> CreatePositiveAdjustmentAsync(CreatePositiveAdjustmentRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreatePositiveAdjustmentResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreatePositiveAdjustmentAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdatePositiveAdjustment")]
    public async Task<ActionResult<ApiSuccessResult<UpdatePositiveAdjustmentResult>>> UpdatePositiveAdjustmentAsync(UpdatePositiveAdjustmentRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdatePositiveAdjustmentResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdatePositiveAdjustmentAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeletePositiveAdjustment")]
    public async Task<ActionResult<ApiSuccessResult<DeletePositiveAdjustmentResult>>> DeletePositiveAdjustmentAsync(DeletePositiveAdjustmentRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeletePositiveAdjustmentResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeletePositiveAdjustmentAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetPositiveAdjustmentList")]
    public async Task<ActionResult<ApiSuccessResult<GetPositiveAdjustmentListResult>>> GetPositiveAdjustmentListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetPositiveAdjustmentListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetPositiveAdjustmentListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetPositiveAdjustmentListAsync)}",
            Content = response
        });
    }



    [Authorize]
    [HttpGet("GetPositiveAdjustmentStatusList")]
    public async Task<ActionResult<ApiSuccessResult<GetPositiveAdjustmentStatusListResult>>> GetPositiveAdjustmentStatusListAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetPositiveAdjustmentStatusListRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetPositiveAdjustmentStatusListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetPositiveAdjustmentStatusListAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetPositiveAdjustmentSingle")]
    public async Task<ActionResult<ApiSuccessResult<GetPositiveAdjustmentSingleResult>>> GetPositiveAdjustmentSingleAsync(
    CancellationToken cancellationToken,
    [FromQuery] string id
    )
    {
        var request = new GetPositiveAdjustmentSingleRequest { Id = id };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetPositiveAdjustmentSingleResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetPositiveAdjustmentSingleAsync)}",
            Content = response
        });
    }

}


