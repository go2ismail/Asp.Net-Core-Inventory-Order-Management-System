using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.TodoManager.Commands;

public class UpdateTodoResult
{
    public Todo? Data { get; set; }
}

public class UpdateTodoRequest : IRequest<UpdateTodoResult>
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateTodoValidator : AbstractValidator<UpdateTodoRequest>
{
    public UpdateTodoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdateTodoHandler : IRequestHandler<UpdateTodoRequest, UpdateTodoResult>
{
    private readonly ICommandRepository<Todo> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTodoHandler(
        ICommandRepository<Todo> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateTodoResult> Handle(UpdateTodoRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.Name = request.Name;
        entity.Description = request.Description;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new UpdateTodoResult
        {
            Data = entity
        };
    }
}

