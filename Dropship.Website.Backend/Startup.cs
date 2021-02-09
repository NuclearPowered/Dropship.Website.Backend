using System;
using System.Text;
using Dropship.Website.Backend.Database;
using Dropship.Website.Backend.Models.Configuration;
using Dropship.Website.Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Dropship.Website.Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<DatabaseContext>(x =>
            {
                x.UseMySql(Configuration.GetConnectionString("Database"),
                    new MariaDbServerVersion(new Version(10, 5, 8)),
                    y => y.MigrationsAssembly("Dropship.Website.Backend"));
            });
            services.AddCors(options =>
            {
                options.AddPolicy(name: "anyorigin", builder => 
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .WithMethods("GET", "POST", "DELETE"));
            });
            
            services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = Configuration.GetConnectionString("Redis");
                    options.InstanceName = "ImpostorBackend";
                });
            // services.AddDistributedMemoryCache();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            var spacesConfig = Configuration.GetSection(SpacesConfig.Section).Get<SpacesConfig>();
            services.Configure<SpacesConfig>(Configuration.GetSection(SpacesConfig.Section));
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", spacesConfig.AccessKey);
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", spacesConfig.SecretKey);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Dropship.Website.Backend", Version = "v1"});
            });

            services.AddSingleton<S3Service>();
            services.AddTransient<AuthService>();
            services.AddTransient<ServerListService>();
            services.AddTransient<ModsService>();
            services.AddTransient<ModBuildsService>();
            services.AddTransient<PluginService>();
            services.AddTransient<PluginBuildService>();
            services.AddTransient<ServerJoinService>();
            services.AddTransient<UploadService>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dropship.Website.Backend v1"));
            }

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseCors("anyorigin");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
