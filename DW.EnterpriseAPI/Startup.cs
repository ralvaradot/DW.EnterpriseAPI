using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DW.EnterpriseAPI.Models;
using DW.EnterpriseAPI.Persistence;
using DW.EnterpriseAPI.Services;
using DW.EnterpriseAPI.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace DW.EnterpriseAPI
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
            services.AddDbContext<EnterpriseContext>(options => 
                options.UseSqlServer(
                    Configuration.GetConnectionString("EnterpriseDB"))
                );
            // Adicionamos la Autenticacion a traves de Entity Framework 
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<EnterpriseContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(
                options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequiredUniqueChars = 1;
                    options.Password.RequireUppercase = true;

                    // Bloqueos
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                }
                );

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();


            // Inyectamos la referencia a traves de la Interfaz
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ICourseService, CourseService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //  Documentar el API
            services.AddSwaggerGen(
                c => c.SwaggerDoc("v1", 
                new Info {
                    Title="REST API Digital Ware",
                    Version="v1",
                    Description="API REST del curso de ASP:NET Core",
                    TermsOfService="http://Termsofservice.digitalware.com.co",
                    Contact = new Contact
                    {
                        Email="info@digitalware.com.co",
                        Name ="Digital Ware S.A.S." ,
                        Url="www.digitalware.com.co"
                    }
                })
                );

            services.AddSingleton<Serilog.ILogger>( options =>
                {
                    var cnnString = Configuration["Serilog:DefaultConnection"];
                    var tableName = Configuration["Serilog:TableName"];
                    return new LoggerConfiguration()
                        .WriteTo
                        .MSSqlServer(cnnString, tableName,
                        restrictedToMinimumLevel:
                        Serilog.Events.LogEventLevel.Warning,
                        autoCreateSqlTable: true)
                        .CreateLogger();
                }
                );

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Log-{Date}.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            // configuramos Swagger
            app.UseSwagger();
            app.UseSwaggerUI(
                c => c.SwaggerEndpoint(
                    "/Swagger/v1/swagger.json",
                    "REST API Digital Ware")
                );

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
