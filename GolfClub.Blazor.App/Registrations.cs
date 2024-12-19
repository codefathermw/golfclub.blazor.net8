using GolfClub.BLL.Services.Authentication;
using GolfClub.BLL.Services.Files;
using GolfClub.BLL.Services.Fittings;
using GolfClub.BLL.Services.Users;
using GolfClub.DAL.Models;
using GolfClub.DAL.Repositories;

namespace GolfClub.Blazor.App
{
    public static class Registrations
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<UserAccount>, GenericRepository<UserAccount>>();
            services.AddScoped<IRepository<UserProfile>, GenericRepository<UserProfile>>();
            services.AddScoped<IRepository<Fitting>, GenericRepository<Fitting>>();
            services.AddScoped<IFittingService, FittingService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            services.AddScoped<IRepository<UserAccount>, GenericRepository<UserAccount>>();
            services.AddScoped<IRepository<UserProfile>, GenericRepository<UserProfile>>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
