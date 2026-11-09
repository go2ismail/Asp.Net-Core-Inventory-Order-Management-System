using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TransferOutManager.Queries;


public class GetTransferOutSingleProfile : Profile
{
    public GetTransferOutSingleProfile()
    {
    }
}

public class GetTransferOutSingleResult
{
    public TransferOut? Data { get; init; }
    public List<InventoryTransaction>? TransactionList { get; init; }
}

public class GetTransferOutSingleRequest : IRequest<GetTransferOutSingleResult>
{
    public string? Id { get; init; }
}

public class GetTransferOutSingleValidator : AbstractValidator<GetTransferOutSingleRequest>
{
    public GetTransferOutSingleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetTransferOutSingleHandler : IRequestHandler<GetTransferOutSingleRequest, GetTransferOutSingleResult>
{
    private readonly IQueryContext _context;

    public GetTransferOutSingleHandler(
        IQueryContext context
        )
    {
        _context = context;
    }

    public async Task<GetTransferOutSingleResult> Handle(GetTransferOutSingleRequest request, CancellationToken cancellationToken)
    {
        var queryData = _context
            .TransferOut
            .AsNoTracking()
            .Include(x => x.WarehouseFrom)
            .Include(x => x.WarehouseTo)
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
            .Where(x => x.ModuleId == request.Id && x.ModuleName == nameof(TransferOut))
            .AsQueryable();

        var transactionList = await queryTransactionList.ToListAsync(cancellationToken);

        return new GetTransferOutSingleResult
        {
            Data = data,
            TransactionList = transactionList
        };
    }
}