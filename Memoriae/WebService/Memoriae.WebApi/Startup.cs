using AutoMapper;
using Memoriae.BAL.Core.Interfaces;
using Memoriae.BAL.PostgreSQL;
using Memoriae.BAL.PostgreSQL.Mapper;
using Memoriae.DAL.PostgreSQL.EF.Models;
using Memoriae.DAL.Users.PostgreSQL.EF;
using Memoriae.Helpers.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

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

            ConfigureCors(services);
            ConfigureMainComponents(services);
            ConfigureMapper(services);
            ConfigureLogger(services);
            ConfigureContext(services);
            ConfigureSwagger(services);
            ConfigureJwt(services);
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
            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(policy =>
            {
                policy.AddPolicy("CorsPolicy", opt => opt
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("X-Pagination"));
            });
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

            services.AddDbContext<UserContext>(
                options => options.UseNpgsql(
                    connection ?? throw new ArgumentNullException(nameof(connection), " has a null connection string."),
                    c => c.MigrationsHistoryTable("__EFMigrationsHistory", schemaName)));

            services.AddIdentity<IdentityUser, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 5;   // минимальная длина
                opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                opts.Password.RequireDigit = false; // требуются ли цифры
            }).AddEntityFrameworkStores<UserContext>();
        }     

        private void ConfigureJwt(IServiceCollection services)
        {
            var jwtSettings = Configuration.GetSection("JwtSettings");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value))
                };
            });
        }

    }
}
