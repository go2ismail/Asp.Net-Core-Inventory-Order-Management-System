using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ScrappingManager.Queries;


public class GetScrappingSingleProfile : Profile
{
    public GetScrappingSingleProfile()
    {
    }
}

public class GetScrappingSingleResult
{
    public Scrapping? Data { get; init; }
    public List<InventoryTransaction>? TransactionList { get; init; }
}

public class GetScrappingSingleRequest : IRequest<GetScrappingSingleResult>
{
    public string? Id { get; init; }
}

public class GetScrappingSingleValidator : AbstractValidator<GetScrappingSingleRequest>
{
    public GetScrappingSingleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetScrappingSingleHandler : IRequestHandler<GetScrappingSingleRequest, GetScrappingSingleResult>
{
    private readonly IQueryContext _context;

    public GetScrappingSingleHandler(
        IQueryContext context
        )
    {
        _context = context;
    }

    public async Task<GetScrappingSingleResult> Handle(GetScrappingSingleRequest request, CancellationToken cancellationToken)
    {
        var queryData = _context
            .Scrapping
            .AsNoTracking()
            .Include(x => x.Warehouse)
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
            .Where(x => x.ModuleId == request.Id && x.ModuleName == nameof(Scrapping))
            .AsQueryable();

        var transactionList = await queryTransactionList.ToListAsync(cancellationToken);

        return new GetScrappingSingleResult
        {
            Data = data,
            TransactionList = transactionList
        };
    }
}