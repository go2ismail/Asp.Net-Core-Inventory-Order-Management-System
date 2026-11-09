using Domain.Common;

namespace Application.Common.Repositories;


public interface ICommandRepository<T> where T : BaseEntity
{
    Task CreateAsync(T entity, CancellationToken cancellationToken = default);

    void Create(T entity);

    void Update(T entity);

    void Delete(T entity);

    void Purge(T entity);

    Task<T?> GetAsync(string id, CancellationToken cancellationToken = default);

    T? Get(string id);

    IQueryable<T> GetQuery();
}

