using FireApi.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using AutoMapper;
using FireApi.Helpers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using FireApi.Services;
using FireApi.Database.MongoDB;
using Microsoft.Extensions.Options;
using FireApi.Database.Repository;

namespace FireApi
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
            services.AddCors();
            services.AddDbContext<DataContext>(options => options.UseMySql(Configuration.GetConnectionString("DataContext"), b => b.MigrationsAssembly("FireApi.Database")),
                                                        ServiceLifetime.Transient);
            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));


            services.AddSingleton<IMongoDbSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);
            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
           {
               x.Events = new JwtBearerEvents
               {
                   OnTokenValidated = context =>
                   {
                       var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                       var userId = Guid.Parse(context.Principal.Identity.Name);
                       var user = userService.GetById(userId);
                       if (user == null)
                       {
                           // return unauthorized if user no longer exists
                           context.Fail("Unauthorized");
                       }
                       return Task.CompletedTask;
                   }
               };
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<IDevicePropService, DevicePropService>();

            services.AddScoped<IClientService, ClientService>();

            services.AddScoped<IFirmService, FirmService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
