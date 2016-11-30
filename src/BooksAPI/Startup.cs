using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BooksAPI.Models;
using Microsoft.EntityFrameworkCore;
using BooksAPI.Services;
using Stormpath.AspNetCore;
using Stormpath.Configuration.Abstractions;

namespace BooksAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddStormpath(new StormpathConfiguration()
            {
                Web = new WebConfiguration()
                {
                    // This explicitly tells the Stormpath middleware to only serve JSON responses (appropriate for an API).
                    // By default, HTML responses are served too.
                    Produces = new[] { "application/json" },
                    Oauth2 = new WebOauth2RouteConfiguration()
                    {
                        Uri = "/token",
                    }
                }
            });
            services.AddDbContext<BooksAPIContext>(x => x.UseInMemoryDatabase());
            services.AddTransient<IBookRepository, InMemoryBookRepository>();
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseStormpath();
            app.UseMvc();
        }
    }
}
