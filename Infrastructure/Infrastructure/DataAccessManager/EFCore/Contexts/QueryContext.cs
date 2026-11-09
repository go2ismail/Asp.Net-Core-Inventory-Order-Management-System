using Application.Common.CQS.Queries;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccessManager.EFCore.Contexts;

public class QueryContext : DataContext, IQueryContext
{
    public QueryContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public new IQueryable<T> Set<T>() where T : class
    {
        return base.Set<T>();
    }
}
