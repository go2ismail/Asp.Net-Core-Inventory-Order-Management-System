using Application.Features.NegativeAdjustmentManager.Commands;
using Application.Features.NegativeAdjustmentManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class NegativeAdjustmentController : BaseApiController
{
    public NegativeAdjustmentController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateNegativeAdjustment")]
    public async Task<ActionResult<ApiSuccessResult<CreateNegativeAdjustmentResult>>> CreateNegativeAdjustmentAsync(CreateNegativeAdjustmentRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateNegativeAdjustmentResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateNegativeAdjustmentAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateNegativeAdjustment")]
    public async Task<ActionResult<ApiSuccessResult<UpdateNegativeAdjustmentResult>>> UpdateNegativeAdjustmentAsync(UpdateNegativeAdjustmentRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateNegativeAdjustmentResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateNegativeAdjustmentAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteNegativeAdjustment")]
    public async Task<ActionResult<ApiSuccessResult<DeleteNegativeAdjustmentResult>>> DeleteNegativeAdjustmentAsync(DeleteNegativeAdjustmentRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteNegativeAdjustmentResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteNegativeAdjustmentAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetNegativeAdjustmentList")]
    public async Task<ActionResult<ApiSuccessResult<GetNegativeAdjustmentListResult>>> GetNegativeAdjustmentListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetNegativeAdjustmentListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetNegativeAdjustmentListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetNegativeAdjustmentListAsync)}",
            Content = response
        });
    }



    [Authorize]
    [HttpGet("GetNegativeAdjustmentStatusList")]
    public async Task<ActionResult<ApiSuccessResult<GetNegativeAdjustmentStatusListResult>>> GetNegativeAdjustmentStatusListAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetNegativeAdjustmentStatusListRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetNegativeAdjustmentStatusListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetNegativeAdjustmentStatusListAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetNegativeAdjustmentSingle")]
    public async Task<ActionResult<ApiSuccessResult<GetNegativeAdjustmentSingleResult>>> GetNegativeAdjustmentSingleAsync(
    CancellationToken cancellationToken,
    [FromQuery] string id
    )
    {
        var request = new GetNegativeAdjustmentSingleRequest { Id = id };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetNegativeAdjustmentSingleResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetNegativeAdjustmentSingleAsync)}",
            Content = response
        });
    }

}


