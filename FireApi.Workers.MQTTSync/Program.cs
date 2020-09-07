using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using FireApi.Database;
using FireApi.Database.MongoDB;
using FireApi.Database.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace FireApi.Workers.MQTTSync
{
    public class Program
    {

        public static Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }
        public static void Main(string[] args)
        {
            var sharedSettings = Path.Combine(Environment.CurrentDirectory, "..", "SharedSettings.json");
            Configuration = new ConfigurationBuilder()
                     .AddJsonFile(sharedSettings)
                     .Build();
            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureServices((hostContext, services) =>
                 {
                     services.AddDbContext<DataContext>(options => options.UseMySql(Configuration.GetConnectionString("DataContext")),
                                                          ServiceLifetime.Transient);
                     services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));
                     services.AddSingleton<IMongoDbSettings>(serviceProvider =>
                         serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);
                     services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
                     services.AddHostedService<Worker>();
                 });
    }
}
