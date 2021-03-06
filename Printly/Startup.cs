using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Printly.Domain.Services.Extensions;
using Printly.Extensions;
using Printly.Middleware;
using Printly.System;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;

namespace Printly
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly AppSettings _appSettings;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _appSettings = new AppSettings();
            Configuration.Bind(_appSettings);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<AppSettings>(_appSettings);
            services.AddPrintlyServices(_appSettings);
            services.AddPrintlyDomainServices(new Domain.Services.MongoDbConfiguration
            {
                ConnectionString = _appSettings.MongoDbStorageConnectionString,
                DatabaseName = _appSettings.MongoDbStorageDatabaseName
            });
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddMediatR(this.GetType().Assembly);
            services.AddControllers();
            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                            new CultureInfo("en-GB")
                    };
                    opts.DefaultRequestCulture = new RequestCulture("en-GB");
                    opts.SupportedCultures = supportedCultures;
                    opts.SupportedUICultures = supportedCultures;
                });
            services.AddLocalization(opts =>
            {
                opts.ResourcesPath = "Resources";
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Printly", Version = "v1" });
                c.CustomSchemaIds(type => type.FullName);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Printly v1");
                });
            }
            app.ConfigureExceptionHandler();
            app.UseRequestLocalization(app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var webSocketOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120)
            };
            app.UseWebSockets(webSocketOptions);
            app.UseMiddleware<TerminalMiddleware>();

            var cancellationTokenSource = new CancellationTokenSource(new TimeSpan(0, 0, 15));
            var systemStateService = app.ApplicationServices.GetService<ISystemStateService>();
            systemStateService.InitialiseAsync(cancellationTokenSource.Token).GetAwaiter().GetResult();
        }

    }
}
