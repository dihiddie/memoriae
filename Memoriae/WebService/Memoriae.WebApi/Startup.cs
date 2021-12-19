using AutoMapper;
using Memoriae.BAL.Core.Interfaces;
using Memoriae.BAL.PostgreSQL;
using Memoriae.BAL.PostgreSQL.Mapper;
using Memoriae.DAL.PostgreSQL.EF.Models;
using Memoriae.Helpers.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;

namespace Memoriae.WebApi
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
            ConfigureMainComponents(services);
            ConfigureMapper(services);
            ConfigureLogger(services);
            ConfigureContext(services);
            ConfigureSwagger(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            var prefix = "swagger";
           app.UseSwagger(c => c.RouteTemplate = "/" + prefix + "/{documentName}/swagger.json");
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = prefix;
                c.SwaggerEndpoint($"/{prefix}/v1/swagger.json", "MemoriaeGateway");
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private void ConfigureMapper(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MemoriaeProfile()));
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(typeof(Startup));
        }

        private void ConfigureMainComponents(IServiceCollection services)
        {
            services.AddScoped<IPostManager, PostManager>();
            services.AddScoped<ITagManager, TagManager>();
        }

        private void ConfigureLogger(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<Startup>>();
            services.AddSingleton(typeof(ILogger), logger);
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c => c.SetDoc(new OpenApiInfo { Title = "Memoriae Api", Version = "v1" }));
            services.AddRouting(x => x.LowercaseUrls = true);
        }

        private void ConfigureContext(IServiceCollection services)
        {           

            var connection = Configuration?.GetSection("DbConnectionOptions")?.GetSection("ConnectionString")?.Value;
            var schemaName = Configuration?.GetSection("DbConnectionOptions")?.GetSection("MigrationSchemaName")?.Value;

            services.AddDbContext<PersonalContext>(
                options => options.UseNpgsql(
                    connection ?? throw new ArgumentNullException(nameof(connection), " has a null connection string."),
                    c => c.MigrationsHistoryTable("__EFMigrationsHistory", schemaName)));
        }     

    }
}
