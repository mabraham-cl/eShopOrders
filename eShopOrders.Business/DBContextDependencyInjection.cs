using eShopOrders.Database.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace eShopOrders.Business
{
    /// <summary>
    /// This class deals with dependency injection for database context
    /// </summary>
    public static class DBContextDependencyInjection
    {
        /// <summary>
        /// This adds dependency injection for Database Context
        /// </summary>
        /// <param name="services">ServiceCollection</param>
        /// <param name="connectionString">Connection string to connect to database</param>
        public static void AddDatabaseContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<SSE_TestContext>(options => options.UseSqlServer(connectionString,
            sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
            }));
        }
    }
}
