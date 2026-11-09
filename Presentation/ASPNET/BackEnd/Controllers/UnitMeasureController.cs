using Application.Features.UnitMeasureManager.Commands;
using Application.Features.UnitMeasureManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class UnitMeasureController : BaseApiController
{
    public UnitMeasureController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateUnitMeasure")]
    public async Task<ActionResult<ApiSuccessResult<CreateUnitMeasureResult>>> CreateUnitMeasureAsync(CreateUnitMeasureRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateUnitMeasureResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateUnitMeasureAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateUnitMeasure")]
    public async Task<ActionResult<ApiSuccessResult<UpdateUnitMeasureResult>>> UpdateUnitMeasureAsync(UpdateUnitMeasureRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateUnitMeasureResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateUnitMeasureAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteUnitMeasure")]
    public async Task<ActionResult<ApiSuccessResult<DeleteUnitMeasureResult>>> DeleteUnitMeasureAsync(DeleteUnitMeasureRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteUnitMeasureResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteUnitMeasureAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetUnitMeasureList")]
    public async Task<ActionResult<ApiSuccessResult<GetUnitMeasureListResult>>> GetUnitMeasureListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetUnitMeasureListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetUnitMeasureListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetUnitMeasureListAsync)}",
            Content = response
        });
    }


}


