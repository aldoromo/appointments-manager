using Appointments.Infrastructure.Interfaces;

namespace Appointments.Api.Middleware
{
    public class SimpleAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public SimpleAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
        {
            if (context.Request.Headers.TryGetValue("X-User-Id", out var userIdHeader)
                && int.TryParse(userIdHeader, out int userId))
            {
                var user = await unitOfWork.Users.GetByIdAsync(userId);
                if (user != null)
                {
                    context.Items["UserId"] = user.UserId;
                    context.Items["UserRole"] = user.Role; // int
                }
            }

            await _next(context);
        }
    }

    // Extensión para registrar el middleware
    public static class SimpleAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseSimpleAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SimpleAuthMiddleware>();
        }
    }
}
