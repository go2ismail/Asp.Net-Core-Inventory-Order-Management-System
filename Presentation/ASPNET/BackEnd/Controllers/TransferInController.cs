using Application.Features.TransferInManager.Commands;
using Application.Features.TransferInManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class TransferInController : BaseApiController
{
    public TransferInController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateTransferIn")]
    public async Task<ActionResult<ApiSuccessResult<CreateTransferInResult>>> CreateTransferInAsync(CreateTransferInRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateTransferInResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateTransferInAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateTransferIn")]
    public async Task<ActionResult<ApiSuccessResult<UpdateTransferInResult>>> UpdateTransferInAsync(UpdateTransferInRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateTransferInResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateTransferInAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteTransferIn")]
    public async Task<ActionResult<ApiSuccessResult<DeleteTransferInResult>>> DeleteTransferInAsync(DeleteTransferInRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteTransferInResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteTransferInAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetTransferInList")]
    public async Task<ActionResult<ApiSuccessResult<GetTransferInListResult>>> GetTransferInListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetTransferInListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetTransferInListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetTransferInListAsync)}",
            Content = response
        });
    }



    [Authorize]
    [HttpGet("GetTransferInStatusList")]
    public async Task<ActionResult<ApiSuccessResult<GetTransferInStatusListResult>>> GetTransferInStatusListAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetTransferInStatusListRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetTransferInStatusListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetTransferInStatusListAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetTransferInSingle")]
    public async Task<ActionResult<ApiSuccessResult<GetTransferInSingleResult>>> GetTransferInSingleAsync(
    CancellationToken cancellationToken,
    [FromQuery] string id
    )
    {
        var request = new GetTransferInSingleRequest { Id = id };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetTransferInSingleResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetTransferInSingleAsync)}",
            Content = response
        });
    }

}


