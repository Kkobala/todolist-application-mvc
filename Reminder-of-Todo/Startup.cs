using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Reminder_of_Todo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList_Application;
using TodoList_Application.UnitOfWork;

namespace Reminder_of_Todo
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
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));;

            services.AddScoped<ITodoListRepository, TodoListRepository>();
            services.AddScoped<SendReminderService>();
            services.AddScoped<IUnitofWork, UnitofWork>();

            services.AddHangfire(configuration => configuration
             .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions()));

            services.AddHangfireServer(x => new BackgroundJobServerOptions { });

            services.AddControllers();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseHangfireDashboard();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });

            ConfigureBackGrounJob(app, env);
        }

        private void ConfigureBackGrounJob(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //RecurringJob.AddOrUpdate<SendReminderService>((job) => job.Execute(), Cron.Daily(01, 00));
            RecurringJob.AddOrUpdate<SendReminderService>((job) => job.Execute(), Cron.Minutely());
        }
    }
}
