using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> InsertAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
           TEntity entity,
           bool autoSave = false,
           CancellationToken cancellationToken = default);

        Task<TEntity> UpdateAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default);

        Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<List<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>>? predicate,
            CancellationToken cancellationToken = default);

        Task<int> GetCountAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);
    }

    public interface IRepository<TKey, TEntity> : IRepository<TEntity>
        where TEntity : Entity<TKey>
    {
        Task DeleteAsync(
            TKey id,
            bool autoSave = false,
            CancellationToken cancellationToken = default);

        Task<TEntity?> GetAsync(
            TKey id,
            CancellationToken cancellationToken = default);
    }
}
