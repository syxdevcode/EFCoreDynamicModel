using DynamicModel.Domain;
using Infrastructure;
using Infrastructure.Implement;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DynamicModel.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("runtimemodelconfig.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //注入获取Provider类
            services.AddSingleton<IRuntimeModelProvider,DefaultRuntimeModelProvider>();

            //services.Configure<RuntimeModelMetaConfig>(Configuration.GetSection("RuntimeModelMetaConfig"));

            services.AddEntityFramework().AddDbContext<DynamicModelDbContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("Default"), sql =>
                {
                    sql.UseRowNumberForPaging();
                    sql.MaxBatchSize(50);
                });
            });


            //获取数据库连接字符串
            var sqlConnectionString = Configuration.GetConnectionString("ModelDb");

            //添加数据上下文
            services.AddEntityFrameworkSqlServer().AddDbContext<ModelDbContext>(options => options.UseSqlServer(sqlConnectionString));


            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}