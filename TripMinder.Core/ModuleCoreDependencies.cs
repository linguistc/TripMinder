using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using MediatR;
using TripMinder.Core.Behaviors;
using TripMinder.Core.Behaviors.Knapsack;
using TripMinder.Core.Behaviors.Shared;


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
            
            // Optimizer Dependencies
            services.AddScoped<IKnapsackSolver, KnapsackSolver>(); 
            services.AddScoped<IKnapsackDP ,KnapsackDP>();
            services.AddScoped<IKnapsackBacktracker, KnapsackBacktracker>();
            services.AddScoped<IProfitFinder, ProfitFinder>();
            services.AddScoped<IDynamicProgrammingCalculator, DynamicProgrammingCalculator>();
            services.AddScoped<IStagedTripPlanOptimizer, StagedTripPlanOptimizer>();
            services.AddScoped<IItemFetcher, ItemFetcher>();
            services.AddScoped<IKnapsackConstraints>(sp => new UserDefinedKnapsackConstraints(3, 1, 3, 3));
            // services.AddScoped<IKnapsackConstraints, UserDefinedKnapsackConstraints>();
            services.AddScoped<TripPlanOptimizer>();
            services.AddScoped<GreedySolutionOptimizer>();
            
            // Greedy Dependencies
            services.AddScoped<IGreedyPhaseOptimizer, GreedyPhaseOptimizer>();
            services.AddScoped<IGreedyTripSolver, GreedyTripSolver>();
            services.AddScoped<GreedyTripPlanner>();
            services.AddScoped<GreedyTripPlanner>();
            services.AddScoped<GreedySolutionOptimizer>();
            services.AddScoped<GreedySolutionCollector>();
            
            
            //
            
            
            return services;
        }

    }
}
