using Application.Common.Repositories;

namespace Application.Common.CQS.Queries;

public interface IQueryContext : IEntityDbSet
{
    IQueryable<T> Set<T>() where T : class;
}

