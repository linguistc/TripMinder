using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using System.Reflection;
using MediatR;
using TripMinder.Core.Behaviors;
using TripMinder.Service.Contracts;

namespace TripMinder.Core
{
    public static class ModuleCoreDependencies
    {
        public static IServiceCollection AddCoreDependecies(this IServiceCollection services)
        {
            // Mediator Configuration
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                Assembly.GetExecutingAssembly()));

            // Auto Mapper Configuration
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            
            // Get Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


            //
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            
            return services;
        }

    }
}
