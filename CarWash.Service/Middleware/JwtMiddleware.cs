using CarWash.Repository.Repositories.Users;
using CarWash.Service.Providers;
using Microsoft.AspNetCore.Http;
namespace CarWash.Service.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserRepository _userRepository, JwtGenerator jwtGenerator)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var validateTokenResult = await jwtGenerator.ValidateToken(token);
            if (validateTokenResult.IsValid)
                context.Items["User"] = await _userRepository.GetByIdAsync(int.Parse(validateTokenResult.UserId));

            await _next(context);
        }

    }
}
