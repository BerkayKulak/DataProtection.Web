using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace MyWeb.API
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
            services.AddCors(opts =>
            {
                opts.AddPolicy("AllowSites", builder =>
                {
                    builder.WithOrigins("https://localhost:44396/","https://www.mysitem.com").AllowAnyHeader().AllowAnyMethod();
                });

                //opts.AddPolicy("AllowSites2", builder =>
                //{
                //    // headerimde b?yle bir ?ey olmazsa kabul etmicem.
                //    builder.WithOrigins("https://www.mysite2.com").WithHeaders(HeaderNames.ContentType,"x-custom-header");
                //});

                //opts.AddPolicy("AllowSites", builder =>
                //{
                //    // ?urdaki * ne olursa olsun, nas?l bir domainden istek geliyorsa gelsin, t?m istekleri, t?m subdomainleri kabul et.
                //    builder.WithOrigins("https://*.example.com").SetIsOriginAllowedToAllowWildcardSubdomains().AllowAnyHeader();
                //});

                opts.AddPolicy("AllowSites2", builder =>
                {
                    builder.WithOrigins("https://www.example.com").WithMethods("POST","GET").AllowAnyHeader();
                });

            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyWeb.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyWeb.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("AllowSites");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
