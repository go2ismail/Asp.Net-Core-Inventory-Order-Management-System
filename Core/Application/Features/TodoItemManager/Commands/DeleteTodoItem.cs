using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.TodoItemManager.Commands;

public class DeleteTodoItemResult
{
    public TodoItem? Data { get; set; }
}

public class DeleteTodoItemRequest : IRequest<DeleteTodoItemResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteTodoItemValidator : AbstractValidator<DeleteTodoItemRequest>
{
    public DeleteTodoItemValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteTodoItemHandler : IRequestHandler<DeleteTodoItemRequest, DeleteTodoItemResult>
{
    private readonly ICommandRepository<TodoItem> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTodoItemHandler(
        ICommandRepository<TodoItem> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteTodoItemResult> Handle(DeleteTodoItemRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteTodoItemResult
        {
            Data = entity
        };
    }
}

