using Application.Features.NumberSequenceManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class NumberSequenceController : BaseApiController
{
    public NumberSequenceController(ISender sender) : base(sender)
    {
    }


    [Authorize]
    [HttpGet("GetNumberSequenceList")]
    public async Task<ActionResult<ApiSuccessResult<GetNumberSequenceListResult>>> GetNumberSequenceListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetNumberSequenceListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetNumberSequenceListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetNumberSequenceListAsync)}",
            Content = response
        });
    }


}


