using Application.Features.TodoManager.Commands;
using Application.Features.TodoManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class TodoController : BaseApiController
{
    public TodoController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateTodo")]
    public async Task<ActionResult<ApiSuccessResult<CreateTodoResult>>> CreateTodoAsync(CreateTodoRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateTodoResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateTodoAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateTodo")]
    public async Task<ActionResult<ApiSuccessResult<UpdateTodoResult>>> UpdateTodoAsync(UpdateTodoRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateTodoResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateTodoAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteTodo")]
    public async Task<ActionResult<ApiSuccessResult<DeleteTodoResult>>> DeleteTodoAsync(DeleteTodoRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteTodoResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteTodoAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetTodoSingle")]
    public async Task<ActionResult<ApiSuccessResult<GetTodoSingleResult>>> GetTodoSingleAsync(
        CancellationToken cancellationToken,
        [FromQuery] string id
        )
    {
        var request = new GetTodoSingleRequest { Id = id };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetTodoSingleResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetTodoSingleAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetTodoList")]
    public async Task<ActionResult<ApiSuccessResult<GetTodoListResult>>> GetTodoListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetTodoListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetTodoListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetTodoListAsync)}",
            Content = response
        });
    }


}


