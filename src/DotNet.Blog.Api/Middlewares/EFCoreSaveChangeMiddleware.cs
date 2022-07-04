using DotNet.Blog.EFCore;

namespace DotNet.Blog.Api.Middlewares
{
    /// <summary>
    /// efcore sava change.  // modified
    /// </summary>
    public class EFCoreSaveChangeMiddleware
    {
        private readonly RequestDelegate _next;

        public EFCoreSaveChangeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, BlogDbContext dbContext)
        {
            await _next(context);

            if (dbContext.ChangeTracker.HasChanges())
            {
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
