using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json.Serialization;

using OSharp.AspNetCore.Mvc.Filters;
using OSharp.Demo.Identity;
using OSharp.Demo.Identity.Entities;
using OSharp.Demo.Web.Startups;
using OSharp.Entity;


namespace OSharp.Demo.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add<UnitOfWorkAttribute>();
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            //services.AddOSharpIdentity<UserStore, RoleStore, User, Role, int, int>()
            //    .AddDefaultTokenProviders();

            services.AddOSharp().AddDistributedMemoryCache().AddLogging(builder =>
            {
                builder.AddFile(ops =>
                {
                    ops.FileName = "log-";
                    ops.LogDirectory = "log";
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages().UseStaticFiles().UseMvcWithAreaRoute()
                .UseOSharpMvc();

            //��ⲢǨ��
            SqlServerDesignTimeDefaultDbContextFactory dbContextFactory = new SqlServerDesignTimeDefaultDbContextFactory();
            var dbContext = dbContextFactory.CreateDbContext(new string[0]);
            dbContext.CheckAndMigration();
        }
    }
}