using Application.Common.CQS.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.BillOfMaterialManager.Queries;

public record BillOfMaterialExplosionDto
{
    public string? ProductId { get; init; }
    public string? ProductName { get; init; }
    public string? ProductNumber { get; init; }
    public int Level { get; init; }
    public double Quantity { get; init; }
    public double TotalQuantity { get; init; }
    public string? UnitMeasureId { get; init; }
    public string? UnitMeasureName { get; init; }
    public double? UnitPrice { get; init; }
    public double? ExtendedCost { get; init; }
    public List<BillOfMaterialExplosionDto>? Children { get; init; }
}

public class GetBillOfMaterialExplosionResult
{
    public BillOfMaterialExplosionDto? Data { get; init; }
    public double? TotalMaterialCost { get; init; }
}

public class GetBillOfMaterialExplosionRequest : IRequest<GetBillOfMaterialExplosionResult>
{
    public string? ProductId { get; init; }
    public double Quantity { get; init; } = 1;
}

public class GetBillOfMaterialExplosionHandler : IRequestHandler<GetBillOfMaterialExplosionRequest, GetBillOfMaterialExplosionResult>
{
    private readonly IQueryContext _context;

    public GetBillOfMaterialExplosionHandler(IQueryContext context)
    {
        _context = context;
    }

    public async Task<GetBillOfMaterialExplosionResult> Handle(GetBillOfMaterialExplosionRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.ProductId))
        {
            throw new Exception("ProductId is required");
        }

        var rootProduct = await _context.Product
            .AsNoTracking()
            .Include(x => x.UnitMeasure)
            .FirstOrDefaultAsync(x => x.Id == request.ProductId && !x.IsDeleted, cancellationToken);

        if (rootProduct == null)
        {
            throw new Exception($"Product not found: {request.ProductId}");
        }

        var explosion = await ExplodeAsync(request.ProductId, request.Quantity, 0, cancellationToken);

        var totalCost = CalculateTotalCost(explosion);

        return new GetBillOfMaterialExplosionResult
        {
            Data = explosion,
            TotalMaterialCost = totalCost
        };
    }

    private async Task<BillOfMaterialExplosionDto> ExplodeAsync(
        string productId, 
        double quantity, 
        int level, 
        CancellationToken cancellationToken)
    {
        var product = await _context.Product
            .AsNoTracking()
            .Include(x => x.UnitMeasure)
            .FirstOrDefaultAsync(x => x.Id == productId && !x.IsDeleted, cancellationToken);

        if (product == null)
        {
            throw new Exception($"Product not found: {productId}");
        }

        var bom = await _context.BillOfMaterials
            .AsNoTracking()
            .Where(x => x.ProductId == productId && !x.IsDeleted && x.IsActive == true)
            .Include(x => x.Items!.Where(i => !i.IsDeleted))
                .ThenInclude(i => i.ComponentProduct)
                    .ThenInclude(p => p!.UnitMeasure)
            .OrderByDescending(x => x.CreatedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

        var children = new List<BillOfMaterialExplosionDto>();

        if (bom?.Items != null && bom.Items.Any())
        {
            var sortedItems = bom.Items.OrderBy(x => x.Sequence ?? int.MaxValue).ToList();

            foreach (var item in sortedItems)
            {
                if (item.ComponentProductId != null)
                {
                    var componentQuantity = (item.Quantity ?? 1) * quantity;
                    var child = await ExplodeAsync(item.ComponentProductId, componentQuantity, level + 1, cancellationToken);
                    children.Add(child);
                }
            }
        }

        return new BillOfMaterialExplosionDto
        {
            ProductId = product.Id,
            ProductName = product.Name,
            ProductNumber = product.Number,
            Level = level,
            Quantity = quantity,
            TotalQuantity = quantity,
            UnitMeasureId = product.UnitMeasureId,
            UnitMeasureName = product.UnitMeasure?.Name,
            UnitPrice = product.UnitPrice,
            ExtendedCost = (product.UnitPrice ?? 0) * quantity,
            Children = children.Any() ? children : null
        };
    }

    private double CalculateTotalCost(BillOfMaterialExplosionDto? node)
    {
        if (node == null)
        {
            return 0;
        }

        double totalCost = 0;

        if (node.Children == null || !node.Children.Any())
        {
            // Leaf node - use its own cost
            totalCost = node.ExtendedCost ?? 0;
        }
        else
        {
            // Parent node - sum up children costs
            foreach (var child in node.Children)
            {
                totalCost += CalculateTotalCost(child);
            }
        }

        return totalCost;
    }
}
