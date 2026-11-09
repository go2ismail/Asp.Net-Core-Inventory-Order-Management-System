using Application.Features.DashboardManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class DashboardController : BaseApiController
{
    public DashboardController(ISender sender) : base(sender)
    {
    }


    [Authorize]
    [HttpGet("GetCardsDashboard")]
    public async Task<ActionResult<ApiSuccessResult<GetCardsDashboardResult>>> GetCardsDashboardAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetCardsDashboardRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetCardsDashboardResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetCardsDashboardAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetSalesDashboard")]
    public async Task<ActionResult<ApiSuccessResult<GetSalesDashboardResult>>> GetSalesDashboardAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetSalesDashboardRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetSalesDashboardResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetSalesDashboardAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetPurchaseDashboard")]
    public async Task<ActionResult<ApiSuccessResult<GetPurchaseDashboardResult>>> GetPurchaseDashboardAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetPurchaseDashboardRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetPurchaseDashboardResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetPurchaseDashboardAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetInventoryDashboard")]
    public async Task<ActionResult<ApiSuccessResult<GetInventoryDashboardResult>>> GetInventoryDashboardAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetInventoryDashboardRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetInventoryDashboardResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetInventoryDashboardAsync)}",
            Content = response
        });
    }



}


