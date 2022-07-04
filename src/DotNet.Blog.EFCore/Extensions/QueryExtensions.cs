using DotNet.Blog.Domain;
using DotNet.Blog.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DotNet.Blog.EFCore.Extensions
{
    public static class QueryExtensions
    {
        public static IQueryable<TEntity> WhereIf<TEntity>(
            this IQueryable<TEntity> query,
            bool condition,
            Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return condition ? query.Where(predicate) : query;
        }

        public static IQueryable<TEntity> IncludeIf<TEntity, TProperty>(
            this IQueryable<TEntity> query, 
            bool condition, 
            Expression<Func<TEntity, TProperty>> path) where TEntity : class
        {
            return condition ? query.Include(path) : query;
        }

        public static IQueryable<TEntity> PageIf<TEntity>(
            this IQueryable<TEntity> query,
            bool condition,
            PageInput input) where TEntity : class
        {
            if (string.IsNullOrEmpty(input.Sorting))
            {
                var type = typeof(TEntity);
                if (type.IsAssignableTo(typeof(ICreationTime)))
                {
                    input.Sorting = $"{nameof(ICreationTime.CreationTime)} DESC";
                }
                else
                {
                    input.Sorting = "1 DESC";
                }
            }

            query = query.OrderBy(input.Sorting);

            return condition ? query.Page(input.Page, input.PageSize) : query;
        }
    }
}
