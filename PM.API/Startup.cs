using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PM.Data.Contexts;
using PM.Domain.Repositories;
using PM.Domain.Services;
using PM.Domain.Infrastructure;

namespace PM.API
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
            services.AddAutoMapper();
            services.AddMvc();
            services.AddDbContext<DataContext>(opts =>
            {
                opts.UseMySQL(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IErrorHandler, ErrorHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
