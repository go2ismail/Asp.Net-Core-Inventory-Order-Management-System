using Application.Features.ScrappingManager.Commands;
using Application.Features.ScrappingManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class ScrappingController : BaseApiController
{
    public ScrappingController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateScrapping")]
    public async Task<ActionResult<ApiSuccessResult<CreateScrappingResult>>> CreateScrappingAsync(CreateScrappingRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateScrappingResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateScrappingAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateScrapping")]
    public async Task<ActionResult<ApiSuccessResult<UpdateScrappingResult>>> UpdateScrappingAsync(UpdateScrappingRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateScrappingResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateScrappingAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteScrapping")]
    public async Task<ActionResult<ApiSuccessResult<DeleteScrappingResult>>> DeleteScrappingAsync(DeleteScrappingRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteScrappingResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteScrappingAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetScrappingList")]
    public async Task<ActionResult<ApiSuccessResult<GetScrappingListResult>>> GetScrappingListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetScrappingListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetScrappingListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetScrappingListAsync)}",
            Content = response
        });
    }




    [Authorize]
    [HttpGet("GetScrappingStatusList")]
    public async Task<ActionResult<ApiSuccessResult<GetScrappingStatusListResult>>> GetScrappingStatusListAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetScrappingStatusListRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetScrappingStatusListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetScrappingStatusListAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetScrappingSingle")]
    public async Task<ActionResult<ApiSuccessResult<GetScrappingSingleResult>>> GetScrappingSingleAsync(
    CancellationToken cancellationToken,
    [FromQuery] string id
    )
    {
        var request = new GetScrappingSingleRequest { Id = id };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetScrappingSingleResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetScrappingSingleAsync)}",
            Content = response
        });
    }
}


