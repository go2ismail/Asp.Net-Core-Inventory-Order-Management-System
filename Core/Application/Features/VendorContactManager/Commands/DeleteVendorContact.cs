using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.VendorContactManager.Commands;

public class DeleteVendorContactResult
{
    public VendorContact? Data { get; set; }
}

public class DeleteVendorContactRequest : IRequest<DeleteVendorContactResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteVendorContactValidator : AbstractValidator<DeleteVendorContactRequest>
{
    public DeleteVendorContactValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteVendorContactHandler : IRequestHandler<DeleteVendorContactRequest, DeleteVendorContactResult>
{
    private readonly ICommandRepository<VendorContact> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVendorContactHandler(
        ICommandRepository<VendorContact> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteVendorContactResult> Handle(DeleteVendorContactRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.DeletedById;

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteVendorContactResult
        {
            Data = entity
        };
    }
}

