using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using Sat.Recruitment.Api.Mappings;
using Sat.Recruitment.Api.Middleware;
using Sat.Recruitment.Application.Commands;
using Sat.Recruitment.Application.Handlers;
using Sat.Recruitment.Application.Interfaces;
using Sat.Recruitment.Application.Services;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.Domain.Interfaces;
using Sat.Recruitment.Domain.Repositories;
using Sat.Recruitment.Domain.ValidationResult;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddAutoMapper(typeof(AutoMapperProfile));

            //services.AddMediatR(typeof(Startup));
            //services.AddTransient<IRequestHandler<CreateUserCommand, User>, CreateUserCommandHandler>();

            // Repository
            services.AddScoped<IUserRepository, UserRepository>();
            // Handlers
            services.AddScoped<ICreateUserCommandHandler, CreateUserCommandHandler>();
            // Services
            services.AddScoped<IUserService, UserService>();

            // Configure NLog for logging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog(Configuration);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //ConfigureLogging(app);
        }

        public void ConfigureLogging(IApplicationBuilder app)
        {
            var config = new NLogLoggingConfiguration(Configuration.GetSection("NLog"));
            LogManager.Configuration = config;
        }
    }
}
