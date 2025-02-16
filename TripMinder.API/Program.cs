using Microsoft.EntityFrameworkCore;
using TripMinder.Infrastructure.Data;
using TripMinder.Infrastructure;
using TripMinder.Service;
using TripMinder.Core;

namespace TripMinder.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Connect to SQL Server
            builder.Services.AddDbContext<AppDBContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DbContext"));
            });

            #region Dependency Injection
            builder.Services.AddInfrastructureDependencies()
                            .AddServiceDependecies()
                            .AddCoreDependecies();
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
