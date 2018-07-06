using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PM.API.Infrastructure.Configurations;
using PM.API.Infrastructure.Middlewares;
using PM.Data.Contexts;
using PM.Domain.Repositories;
using PM.Domain.Services;

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
            services.AddMvc().AddFluentValidation(fv => 
                fv.RegisterValidatorsFromAssemblyContaining<Startup>()
            );
            services.AddDbContext<DataContext>(opts =>
            {
                opts.UseMySQL(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IBatchService, BatchService>();
            services.Configure<Messages>(Configuration.GetSection("Messages"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddLog4Net();

            app.UseErrorHandlingMiddleware();
            app.UseLoggingMiddleware();

            app.UseMvc();
        }
    }
}
