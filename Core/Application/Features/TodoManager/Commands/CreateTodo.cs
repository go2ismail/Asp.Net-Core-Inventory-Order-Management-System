using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.TodoManager.Commands;

public class CreateTodoResult
{
    public Todo? Data { get; set; }
}

public class CreateTodoRequest : IRequest<CreateTodoResult>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateTodoValidator : AbstractValidator<CreateTodoRequest>
{
    public CreateTodoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateTodoHandler : IRequestHandler<CreateTodoRequest, CreateTodoResult>
{
    private readonly ICommandRepository<Todo> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTodoHandler(
        ICommandRepository<Todo> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateTodoResult> Handle(CreateTodoRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new Todo();
        entity.CreatedById = request.CreatedById;

        entity.Name = request.Name;
        entity.Description = request.Description;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateTodoResult
        {
            Data = entity
        };
    }
}