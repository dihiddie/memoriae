using System;
using System.Linq;
using System.Linq.Expressions;

namespace Memoriae.BAL.PostgreSQL.Extensions
{
    public static class QueryableExtensions
    {
        //public static IQueryable<T> Paging<T>(this IQueryable<T> query, PagingModel pagingModel)
        //{
        //    if (pagingModel == null) return query;
        //    if (pagingModel.NoLimit) return query;
        //    if (pagingModel.Start > 0)
        //        query = query.Skip(pagingModel.Start);

        //    if (pagingModel.Limit >= 0)
        //        query = query.Take(pagingModel.Limit);

        //    return query;
        //}

        public static IQueryable<TSource> WhereIf<TSource>(
            this IQueryable<TSource> source,
            bool condition,
            Expression<Func<TSource, bool>> predicate) =>
            condition ? source.Where(predicate) : source;
    }
}
