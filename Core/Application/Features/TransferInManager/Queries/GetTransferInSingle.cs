using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TransferInManager.Queries;


public class GetTransferInSingleProfile : Profile
{
    public GetTransferInSingleProfile()
    {
    }
}

public class GetTransferInSingleResult
{
    public TransferIn? Data { get; init; }
    public List<InventoryTransaction>? TransactionList { get; init; }
}

public class GetTransferInSingleRequest : IRequest<GetTransferInSingleResult>
{
    public string? Id { get; init; }
}

public class GetTransferInSingleValidator : AbstractValidator<GetTransferInSingleRequest>
{
    public GetTransferInSingleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetTransferInSingleHandler : IRequestHandler<GetTransferInSingleRequest, GetTransferInSingleResult>
{
    private readonly IQueryContext _context;

    public GetTransferInSingleHandler(
        IQueryContext context
        )
    {
        _context = context;
    }

    public async Task<GetTransferInSingleResult> Handle(GetTransferInSingleRequest request, CancellationToken cancellationToken)
    {
        var queryData = _context
            .TransferIn
            .AsNoTracking()
            .Include(x => x.TransferOut)
                .ThenInclude(x => x.WarehouseFrom)
            .Include(x => x.TransferOut)
                .ThenInclude(x => x.WarehouseTo)
            .Where(x => x.Id == request.Id)
            .AsQueryable();

        var data = await queryData.SingleOrDefaultAsync(cancellationToken);


        var queryTransactionList = _context
            .InventoryTransaction
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Include(x => x.Product)
            .Include(x => x.Warehouse)
            .Include(x => x.WarehouseFrom)
            .Include(x => x.WarehouseTo)
            .Where(x => x.ModuleId == request.Id && x.ModuleName == nameof(TransferIn))
            .AsQueryable();

        var transactionList = await queryTransactionList.ToListAsync(cancellationToken);

        return new GetTransferInSingleResult
        {
            Data = data,
            TransactionList = transactionList
        };
    }
}