using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireApi.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FireApi.Workers.MQTTSync
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureServices((hostContext, services) =>
                 {
                     var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                     optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FireContext;Trusted_Connection=True;MultipleActiveResultSets=true");
                     services.AddScoped<DataContext>(s => new DataContext(optionsBuilder.Options));

                     services.AddHostedService<Worker>();
                 });
    }
}
