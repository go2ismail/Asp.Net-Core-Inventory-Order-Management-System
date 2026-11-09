using Domain.Common;

namespace Application.Common.Extensions;


public static class QueryableExtensions
{
    public static IQueryable<T> ApplyIsDeletedFilter<T>(this IQueryable<T> query, bool isDeleted = false) where T : class
    {
        if (typeof(IHasIsDeleted).IsAssignableFrom(typeof(T)))
        {
            query = query.Where(x => (x as IHasIsDeleted)!.IsDeleted == isDeleted);
        }

        return query;
    }

}


