using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Identity;
using Task4.Models;

namespace Task4.Middlewares
{
    public class LoginTimeAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

        private readonly UserManager<User> userManager;

        public LoginTimeAuthorizationMiddlewareResultHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var user = await userManager.GetUserAsync(context.User);
                user.LastLogin = DateTime.Now;
                await userManager.UpdateAsync(user);
            }

            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}