using DotNet.Blog.Domain;
using DotNet.Blog.Domain.Shared;
using DotNet.Blog.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DotNet.Blog.EFCore
{
    public class EFCoreCategoryRepository : EFCoreRepository<Guid, Category>, ICategoryRepository
    {
        public EFCoreCategoryRepository(BlogDbContext blogDbContext) : base(blogDbContext)
        {
        }


        public async Task<int> GetCountAsync(GetCategoriesInput input, CancellationToken cancellationToken = default)
        {
            var query = Build(input);
            return await query.CountAsync(cancellationToken);
        }

        public async Task<List<Category>> GetListAsync(GetCategoriesInput input, CancellationToken cancellationToken = default)
        {
            return await Build(input,true).ToListAsync(cancellationToken);
        }


        #region private methods

        private IQueryable<Category> Build(GetCategoriesInput input, bool paged = false)
        {
            IQueryable<Category> query = DbSet
                .WhereIf(!string.IsNullOrEmpty(input.Name), c => c.Name.Contains(input.Name!))
                .PageIf(paged, input);

            return query;
        }

        #endregion
    }
}
