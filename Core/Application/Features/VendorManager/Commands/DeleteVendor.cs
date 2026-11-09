using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.VendorManager.Commands;

public class DeleteVendorResult
{
    public Vendor? Data { get; set; }
}

public class DeleteVendorRequest : IRequest<DeleteVendorResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteVendorValidator : AbstractValidator<DeleteVendorRequest>
{
    public DeleteVendorValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteVendorHandler : IRequestHandler<DeleteVendorRequest, DeleteVendorResult>
{
    private readonly ICommandRepository<Vendor> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVendorHandler(
        ICommandRepository<Vendor> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteVendorResult> Handle(DeleteVendorRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteVendorResult
        {
            Data = entity
        };
    }
}

