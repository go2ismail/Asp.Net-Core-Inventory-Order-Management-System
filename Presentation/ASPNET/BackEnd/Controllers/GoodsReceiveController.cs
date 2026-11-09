using Application.Features.GoodsReceiveManager.Commands;
using Application.Features.GoodsReceiveManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class GoodsReceiveController : BaseApiController
{
    public GoodsReceiveController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateGoodsReceive")]
    public async Task<ActionResult<ApiSuccessResult<CreateGoodsReceiveResult>>> CreateGoodsReceiveAsync(CreateGoodsReceiveRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateGoodsReceiveResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateGoodsReceiveAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateGoodsReceive")]
    public async Task<ActionResult<ApiSuccessResult<UpdateGoodsReceiveResult>>> UpdateGoodsReceiveAsync(UpdateGoodsReceiveRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateGoodsReceiveResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateGoodsReceiveAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteGoodsReceive")]
    public async Task<ActionResult<ApiSuccessResult<DeleteGoodsReceiveResult>>> DeleteGoodsReceiveAsync(DeleteGoodsReceiveRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteGoodsReceiveResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteGoodsReceiveAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetGoodsReceiveList")]
    public async Task<ActionResult<ApiSuccessResult<GetGoodsReceiveListResult>>> GetGoodsReceiveListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetGoodsReceiveListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetGoodsReceiveListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetGoodsReceiveListAsync)}",
            Content = response
        });
    }



    [Authorize]
    [HttpGet("GetGoodsReceiveStatusList")]
    public async Task<ActionResult<ApiSuccessResult<GetGoodsReceiveStatusListResult>>> GetGoodsReceiveStatusListAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetGoodsReceiveStatusListRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetGoodsReceiveStatusListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetGoodsReceiveStatusListAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetGoodsReceiveSingle")]
    public async Task<ActionResult<ApiSuccessResult<GetGoodsReceiveSingleResult>>> GetGoodsReceiveSingleAsync(
    CancellationToken cancellationToken,
    [FromQuery] string id
    )
    {
        var request = new GetGoodsReceiveSingleRequest { Id = id };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetGoodsReceiveSingleResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetGoodsReceiveSingleAsync)}",
            Content = response
        });
    }

}


