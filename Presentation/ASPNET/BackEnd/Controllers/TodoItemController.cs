using Application.Features.TodoItemManager.Commands;
using Application.Features.TodoItemManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class TodoItemController : BaseApiController
{
    public TodoItemController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("CreateTodoItem")]
    public async Task<ActionResult<ApiSuccessResult<CreateTodoItemResult>>> CreateTodoItemAsync(CreateTodoItemRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateTodoItemResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateTodoItemAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateTodoItem")]
    public async Task<ActionResult<ApiSuccessResult<UpdateTodoItemResult>>> UpdateTodoItemAsync(UpdateTodoItemRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateTodoItemResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateTodoItemAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeleteTodoItem")]
    public async Task<ActionResult<ApiSuccessResult<DeleteTodoItemResult>>> DeleteTodoItemAsync(DeleteTodoItemRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteTodoItemResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteTodoItemAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetTodoItemSingle")]
    public async Task<ActionResult<ApiSuccessResult<GetTodoItemSingleResult>>> GetTodoItemSingleAsync(
        CancellationToken cancellationToken,
        [FromQuery] string id
        )
    {
        var request = new GetTodoItemSingleRequest { Id = id };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetTodoItemSingleResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetTodoItemSingleAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GetTodoItemList")]
    public async Task<ActionResult<ApiSuccessResult<GetTodoItemListResult>>> GetTodoItemListAsync(
        CancellationToken cancellationToken,
        [FromQuery] bool isDeleted = false
        )
    {
        var request = new GetTodoItemListRequest { IsDeleted = isDeleted };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetTodoItemListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetTodoItemListAsync)}",
            Content = response
        });
    }


}


