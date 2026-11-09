using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.TodoItemManager.Commands;

public class UpdateTodoItemResult
{
    public TodoItem? Data { get; set; }
}

public class UpdateTodoItemRequest : IRequest<UpdateTodoItemResult>
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? TodoId { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateTodoItemValidator : AbstractValidator<UpdateTodoItemRequest>
{
    public UpdateTodoItemValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.TodoId).NotEmpty();
    }
}

public class UpdateTodoItemHandler : IRequestHandler<UpdateTodoItemRequest, UpdateTodoItemResult>
{
    private readonly ICommandRepository<TodoItem> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTodoItemHandler(
        ICommandRepository<TodoItem> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateTodoItemResult> Handle(UpdateTodoItemRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.TodoId = request.TodoId;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new UpdateTodoItemResult
        {
            Data = entity
        };
    }
}

