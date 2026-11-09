using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.TodoItemManager.Commands;

public class CreateTodoItemResult
{
    public TodoItem? Data { get; set; }
}

public class CreateTodoItemRequest : IRequest<CreateTodoItemResult>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? TodoId { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateTodoItemValidator : AbstractValidator<CreateTodoItemRequest>
{
    public CreateTodoItemValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.TodoId).NotEmpty();
    }
}

public class CreateTodoItemHandler : IRequestHandler<CreateTodoItemRequest, CreateTodoItemResult>
{
    private readonly ICommandRepository<TodoItem> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTodoItemHandler(
        ICommandRepository<TodoItem> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateTodoItemResult> Handle(CreateTodoItemRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new TodoItem();
        entity.CreatedById = request.CreatedById;

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.TodoId = request.TodoId;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateTodoItemResult
        {
            Data = entity
        };
    }
}