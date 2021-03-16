using eShopOrders.Business;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopOrders.API
{
    [ExcludeFromCodeCoverage]
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

            services.AddDatabaseContext(Configuration.GetConnectionString("DefaultConnection"));

            // Adding DI Services

            services.AddTransient<IOrderService, OrderService>();

            services.AddTransient<ICustomerService, CustomerService>();

            services.AddTransient<ICustomerOrderService, CustomerOrderService>();

            // Configure HttpClient for CustomerService
            services.AddHttpClient<ICustomerService, CustomerService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["CustomerUrl"]);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

            // Adding Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = $"1.0",
                    Title = "eShopOrders API",
                    Description = "An API that retrieves the details about delivery of most recent order for a customer.",
                    Contact = new OpenApiContact
                    {
                        Name = "eShopOrders",
                        Email = string.Empty                        
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                // Add the generated documentation xml files

                var basePath = AppContext.BaseDirectory;

                var apiXmlPath = System.IO.Path.Combine(basePath, "eShopOrders.API.xml");
                var sharedlXmlPath = System.IO.Path.Combine(basePath, "eShopOrders.Shared.xml");

                // Only include if the files actually exist
                if (System.IO.File.Exists(apiXmlPath))
                    c.IncludeXmlComments(apiXmlPath);
                if (System.IO.File.Exists(sharedlXmlPath))
                    c.IncludeXmlComments(sharedlXmlPath);

            });

            services.AddSwaggerGenNewtonsoftSupport();

        }

        /// <summary>
        /// Add the Circuit Breaker policy to the list of policies
        /// This is for better handling of faults when trying to connect to a remote service.
        /// This can improve the stability and resiliency of the application.
        /// </summary>
        /// <returns>Policy Interface</returns>
        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }

        /// <summary>
        /// Policy for retrying an action
        /// Configured for 500 as any unexpected failure is returned as 500
        /// </summary>
        /// <returns>Policy Interface</returns>
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "eShopOrders API V1");

                // Serve the Swagger UI at the app's root
                c.RoutePrefix = string.Empty;
            });

        }
    }
}
