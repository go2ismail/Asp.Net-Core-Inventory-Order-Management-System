using Application.Features.PurchaseReturnManager.Commands;
using Application.Features.PurchaseReturnManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class PurchaseReturnController : BaseApiController
{
    public PurchaseReturnController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreatePurchaseReturn")]
    public async Task<ActionResult<ApiSuccessResult<CreatePurchaseReturnResult>>> CreatePurchaseReturnAsync(CreatePurchaseReturnRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreatePurchaseReturnResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreatePurchaseReturnAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdatePurchaseReturn")]
    public async Task<ActionResult<ApiSuccessResult<UpdatePurchaseReturnResult>>> UpdatePurchaseReturnAsync(UpdatePurchaseReturnRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdatePurchaseReturnResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdatePurchaseReturnAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeletePurchaseReturn")]
    public async Task<ActionResult<ApiSuccessResult<DeletePurchaseReturnResult>>> DeletePurchaseReturnAsync(DeletePurchaseReturnRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeletePurchaseReturnResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeletePurchaseReturnAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetPurchaseReturnList")]
    public async Task<ActionResult<ApiSuccessResult<GetPurchaseReturnListResult>>> GetPurchaseReturnListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetPurchaseReturnListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetPurchaseReturnListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetPurchaseReturnListAsync)}",
            Content = response
        });
    }



    [Authorize]
    [HttpGet("GetPurchaseReturnStatusList")]
    public async Task<ActionResult<ApiSuccessResult<GetPurchaseReturnStatusListResult>>> GetPurchaseReturnStatusListAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetPurchaseReturnStatusListRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetPurchaseReturnStatusListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetPurchaseReturnStatusListAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetPurchaseReturnSingle")]
    public async Task<ActionResult<ApiSuccessResult<GetPurchaseReturnSingleResult>>> GetPurchaseReturnSingleAsync(
    CancellationToken cancellationToken,
    [FromQuery] string id
    )
    {
        var request = new GetPurchaseReturnSingleRequest { Id = id };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetPurchaseReturnSingleResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetPurchaseReturnSingleAsync)}",
            Content = response
        });
    }

}


