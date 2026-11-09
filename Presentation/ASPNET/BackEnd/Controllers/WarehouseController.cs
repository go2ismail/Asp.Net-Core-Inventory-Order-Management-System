using Application.Features.WarehouseManager.Commands;
using Application.Features.WarehouseManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class WarehouseController : BaseApiController
{
    public WarehouseController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateWarehouse")]
    public async Task<ActionResult<ApiSuccessResult<CreateWarehouseResult>>> CreateWarehouseAsync(CreateWarehouseRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateWarehouseResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateWarehouseAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateWarehouse")]
    public async Task<ActionResult<ApiSuccessResult<UpdateWarehouseResult>>> UpdateWarehouseAsync(UpdateWarehouseRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateWarehouseResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateWarehouseAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteWarehouse")]
    public async Task<ActionResult<ApiSuccessResult<DeleteWarehouseResult>>> DeleteWarehouseAsync(DeleteWarehouseRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteWarehouseResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteWarehouseAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetWarehouseList")]
    public async Task<ActionResult<ApiSuccessResult<GetWarehouseListResult>>> GetWarehouseListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetWarehouseListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetWarehouseListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetWarehouseListAsync)}",
            Content = response
        });
    }


}


