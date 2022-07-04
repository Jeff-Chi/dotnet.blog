namespace DotNet.Blog.Api
{
    public class ConsoleMiddleware
    {
        private readonly RequestDelegate _next;
        public ConsoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
            Console.WriteLine("中间件测试...");
            Console.WriteLine(Console.ForegroundColor);
        }
    }
}
