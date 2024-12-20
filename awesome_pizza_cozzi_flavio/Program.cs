using Asp.Versioning;
using awesome_pizza.Application.Mediatr;
using awesome_pizza.Domain.Repositories;
using awesome_pizza.Infrastructure.Persistence.EfCore;
using awesome_pizza.Infrastructure.Persistence.EfCore.Repositories;
using awesome_pizza_cozzi_flavio.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Reflection;

namespace awesome_pizza_cozzi_flavio
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.

            builder.Services.AddControllers();

            //  swagger
            builder.Services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = false;
                setup.ReportApiVersions = true;
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

            //  dbcontext
            builder.Services.AddDbContext<IUnitOfWork, AwesomePizzaDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    serverOptions =>
                    {
                        serverOptions.EnableRetryOnFailure(3);
                    });
            });

            //  mediatr
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Mediatr).Assembly));

            //  repositories
            builder.Services.AddScoped<IPizzaRepository, PizzaRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();


            //  mapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //  db connection
            builder.Services.AddScoped<IDbConnection>(conf => new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }

    public partial class Program { }
}
