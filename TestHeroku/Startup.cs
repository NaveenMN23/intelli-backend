using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Middleware;
using IntelliCRMAPIService.Repository;
using IntelliCRMAPIService.Services;
using IntelliCRMAPIService.Utility;
using System.Text;
using IntelliCRMAPIService.BL;

namespace IntelliCRMAPIService
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }
        private IWebHostEnvironment CurrentEnvironment { get; set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //var _logger = services.
            //_logger.LogError("ConfigureServices");
            //services.AddCors(options =>
            //{
            //    //options.AddPolicy(name: "AllowOrigin",
            //    //    builder =>
            //    //    {
            //    //        builder.WithOrigins(Configuration.GetValue<string>("Origin"))
            //    //                            .AllowAnyHeader()
            //    //                            .AllowAnyMethod();
            //    //    });
            //});

            services.AddAuthentication().AddJwtBearer("categories_auth_scheme", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JwtTokenSettings:JwtSecretKey"))),
                    ValidAudience = Configuration.GetValue<string>("JwtTokenSettings:Audience"),
                    ValidIssuer = Configuration.GetValue<string>("JwtTokenSettings:Issuer"),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                };

            });

            var connectionString = Configuration.GetConnectionString("IntelliCRMDb");
            var connectionStringPostgres = Configuration.GetConnectionString("WebApiDatabase");

            if (!CurrentEnvironment.IsDevelopment())
            {
                //_logger.LogError($"ConfigureServices env--{Environment.GetEnvironmentVariable("DATABASE_URL")}");
                var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                connectionUrl = connectionUrl.Replace("postgres://", string.Empty);
                var userPassSide = connectionUrl.Split("@")[0];
                var hostSide = connectionUrl.Split("@")[1];

                var user = userPassSide.Split(":")[0];
                var password = userPassSide.Split(":")[1];
                var host = hostSide.Split("/")[0];
                var database = hostSide.Split("/")[1].Split("?")[0];

                connectionStringPostgres = $"Host={host};Database={database};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";

            }
            //services.AddDbContext<ApplicationDBContext>(options =>
            //    options.UseSqlServer(connectionString));

            //services.AddDbContext<ApplicationDBContext>(options =>
            //    options.UseSqlServer(connectionString));

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddDbContext<PostgresDBContext>(options =>
                options.UseNpgsql(connectionStringPostgres));

            services.AddControllers();
            //services.AddHealthChecks();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IntelliCRM API", Version = "v1" });
                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });
            });

           

            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IAuthTokenGenerator, AuthTokenGenerator>();
            services.AddScoped<IAPIUsersRepository, APIUsersService>();
            services.AddTransient<IUserDetailsRepository, UserDetailsRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ISuperAdminRepository, SuperAdminRepository>();
            services.AddTransient<ISuperAdminBL, SuperAdminBL>();
            services.AddTransient<ICustomerProductRepository, CustomerProductRepository>();
            services.AddTransient<ExcelConverter>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductBL, ProductBL>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            //if (env.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            //app.UseCors(x => x.SetIsOriginAllowed(origin => origin.Contains(Configuration.GetValue<string>("Origin"))));
            //app.UseCors("AllowOrigin");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAuthenticationMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
