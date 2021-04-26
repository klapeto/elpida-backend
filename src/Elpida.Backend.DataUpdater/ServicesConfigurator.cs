using System;
using Elpida.Backend.Data;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Elpida.Backend.DataUpdater
{
    internal class ServicesConfigurator
    {
        public IServiceProvider ServiceProvider { get; }
        
        public IConfigurationRoot Configuration { get; }

        public ServicesConfigurator(string[] args)
        {
            var services = new ServiceCollection();
            
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddCommandLine(args)
                .Build();

            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddConfiguration(Configuration);
            });
            
            services.AddScoped<IBenchmarkResultsService, BenchmarkResultService>();
            services.AddScoped<IBenchmarkService, BenchmarkService>();
            services.AddScoped<ICpuService, CpuService>();
            services.AddScoped<IElpidaService, ElpidaService>();
            services.AddScoped<IOsService, OsService>();
            services.AddScoped<ITaskStatisticsService, TaskTaskStatisticsService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITopologyService, TopologyService>();


            services.AddTransient<IBenchmarkResultsRepository, BenchmarkResultsRepository>();
            services.AddTransient<ICpuRepository, CpuRepository>();
            services.AddTransient<ITopologyRepository, TopologyRepository>();
            services.AddTransient<IBenchmarkRepository, BenchmarkRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<IElpidaRepository, ElpidaRepository>();
            services.AddTransient<IOsRepository, OsRepository>();
            services.AddTransient<ITaskStatisticsRepository, TaskStatisticsRepository>();
            
            services.AddDbContext<ElpidaContext>(builder =>
            {
                builder.UseSqlite(Configuration.GetConnectionString("Local"));
            });
            
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}