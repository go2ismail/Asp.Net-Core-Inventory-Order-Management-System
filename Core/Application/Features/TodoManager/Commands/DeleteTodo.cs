using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.TodoManager.Commands;

public class DeleteTodoResult
{
    public Todo? Data { get; set; }
}

public class DeleteTodoRequest : IRequest<DeleteTodoResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteTodoValidator : AbstractValidator<DeleteTodoRequest>
{
    public DeleteTodoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteTodoHandler : IRequestHandler<DeleteTodoRequest, DeleteTodoResult>
{
    private readonly ICommandRepository<Todo> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTodoHandler(
        ICommandRepository<Todo> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteTodoResult> Handle(DeleteTodoRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteTodoResult
        {
            Data = entity
        };
    }
}

