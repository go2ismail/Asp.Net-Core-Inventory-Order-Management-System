using Application.Features.TaxManager.Commands;
using Application.Features.TaxManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class TaxController : BaseApiController
{
    public TaxController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateTax")]
    public async Task<ActionResult<ApiSuccessResult<CreateTaxResult>>> CreateTaxAsync(CreateTaxRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateTaxResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateTaxAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateTax")]
    public async Task<ActionResult<ApiSuccessResult<UpdateTaxResult>>> UpdateTaxAsync(UpdateTaxRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateTaxResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateTaxAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteTax")]
    public async Task<ActionResult<ApiSuccessResult<DeleteTaxResult>>> DeleteTaxAsync(DeleteTaxRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteTaxResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteTaxAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetTaxList")]
    public async Task<ActionResult<ApiSuccessResult<GetTaxListResult>>> GetTaxListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetTaxListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetTaxListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetTaxListAsync)}",
            Content = response
        });
    }


}


