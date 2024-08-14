using Microsoft.AspNetCore.Authentication.Cookies;

namespace GolfClub.Blazor.App
{
    public static class Configurations
    {
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "access_token";
                    options.Cookie.MaxAge = TimeSpan.FromMinutes(60);
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/forbidden";
                });

            services.AddAuthorizationBuilder()
                .AddPolicy("MustBeAdmin", policy =>
                    policy.RequireRole("Admin"));

            services.AddCascadingAuthenticationState();

            return services;
        }
    }
}
