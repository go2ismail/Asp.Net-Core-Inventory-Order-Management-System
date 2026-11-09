using Application.Features.TransferOutManager.Commands;
using Application.Features.TransferOutManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class TransferOutController : BaseApiController
{
    public TransferOutController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateTransferOut")]
    public async Task<ActionResult<ApiSuccessResult<CreateTransferOutResult>>> CreateTransferOutAsync(CreateTransferOutRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateTransferOutResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateTransferOutAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateTransferOut")]
    public async Task<ActionResult<ApiSuccessResult<UpdateTransferOutResult>>> UpdateTransferOutAsync(UpdateTransferOutRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateTransferOutResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateTransferOutAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteTransferOut")]
    public async Task<ActionResult<ApiSuccessResult<DeleteTransferOutResult>>> DeleteTransferOutAsync(DeleteTransferOutRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteTransferOutResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteTransferOutAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetTransferOutList")]
    public async Task<ActionResult<ApiSuccessResult<GetTransferOutListResult>>> GetTransferOutListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetTransferOutListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetTransferOutListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetTransferOutListAsync)}",
            Content = response
        });
    }



    [Authorize]
    [HttpGet("GetTransferOutStatusList")]
    public async Task<ActionResult<ApiSuccessResult<GetTransferOutStatusListResult>>> GetTransferOutStatusListAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetTransferOutStatusListRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetTransferOutStatusListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetTransferOutStatusListAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetTransferOutSingle")]
    public async Task<ActionResult<ApiSuccessResult<GetTransferOutSingleResult>>> GetTransferOutSingleAsync(
    CancellationToken cancellationToken,
    [FromQuery] string id
    )
    {
        var request = new GetTransferOutSingleRequest { Id = id };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetTransferOutSingleResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetTransferOutSingleAsync)}",
            Content = response
        });
    }

}


