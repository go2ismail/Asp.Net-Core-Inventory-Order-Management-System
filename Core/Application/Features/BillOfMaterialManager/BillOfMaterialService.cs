using Application.Common.CQS.Queries;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.BillOfMaterialManager;

public class BillOfMaterialService
{
    private readonly IQueryContext _context;

    public BillOfMaterialService(IQueryContext context)
    {
        _context = context;
    }

    public async Task<bool> ValidateCircularReferenceAsync(string parentProductId, string componentProductId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(parentProductId) || string.IsNullOrEmpty(componentProductId))
        {
            return false;
        }

        if (parentProductId == componentProductId)
        {
            return true; // direct circular reference
        }

        var visited = new HashSet<string>();
        return await HasPathToAsync(componentProductId, parentProductId, visited, cancellationToken);
    }

    private async Task<bool> HasPathToAsync(string startProductId, string targetProductId, HashSet<string> visited, CancellationToken cancellationToken)
    {
        if (startProductId == targetProductId)
        {
            return true;
        }

        if (visited.Contains(startProductId))
        {
            return false;
        }
        visited.Add(startProductId);

        var bom = await _context.BillOfMaterials
            .AsNoTracking()
            .Where(x => x.ProductId == startProductId && !x.IsDeleted && x.IsActive == true)
            .Include(x => x.Items!.Where(i => !i.IsDeleted))
            .FirstOrDefaultAsync(cancellationToken);

        if (bom?.Items == null || !bom.Items.Any())
        {
            return false;
        }

        foreach (var item in bom.Items)
        {
            if (item.ComponentProductId == null)
            {
                continue;
            }

            if (item.ComponentProductId == targetProductId)
            {
                return true;
            }

            var found = await HasPathToAsync(item.ComponentProductId, targetProductId, visited, cancellationToken);
            if (found)
            {
                return true;
            }
        }

        return false;
    }
}
