using Contracts;
using LoggerService;
using Repository;
using Microsoft.EntityFrameworkCore;
using CompanyEmployees.CustomFormatter;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Marvin.Cache.Headers;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Entities.ConfigurationModels;
using Microsoft.OpenApi.Models;

namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
        /*a code that allows all requests from all origins to be sent to our API
         we should be more  restrictive with those settings in the production environment.
         More precisely, as restrictive as possible.
         */

        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    /*to enable the client application to read the new X-Pagination
                        header that we’ve added in our action, we have to modify the CORS
                    configuration*/
                    .WithExposedHeaders("X-Pagination")
                    );


                });

        /*ASP.NET Core applications are by default self-hosted, and if we want to
host our application on IIS, we need to configure an IIS integration which
will eventually help us with the deployment to IIS. To do that, we need to
add the following code to the ServiceExtensions class*/
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
                services.Configure<IISOptions>(options =>
                {
                });

        /*Inject The Logger Manager Service */
        public static void ConfigureLoggerService(this IServiceCollection services) =>
 services.AddSingleton<ILoggerManager, LoggerManager>();

        /*a repository manager class,
which will create instances of repository user classes for us and then
register them inside the dependency injection container*/
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
services.AddScoped<IRepositoryManager, RepositoryManager>();

        
        /*RepositoryManager service
registration, which happens at runtime, and during that registration, we
must have RepositoryContext registered as well in the runtime, so we
could inject it into other services (like RepositoryManager service)*/
        public static void ConfigureSqlContext(this IServiceCollection services,
IConfiguration configuration) =>
services.AddDbContext<RepositoryContext>(opts =>
opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

        /*Services Extensions for the customer output formatter */
        public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) =>
 builder.AddMvcOptions(config => config.OutputFormatters.Add(new
 CsvOutputFormatter()));

        /*register our new custom media types in the middleware.
Otherwise, we’ll just get a 406 Not Acceptable message
        We are registering two new custom media types for the JSON and XML
output formatters. This ensures we don’t get a 406 Not Acceptable
response*/
        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config.OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();
                if (systemTextJsonOutputFormatter != null)
                {
                    systemTextJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.codemaze.hateoas+json");/*What we want to do is to enable links in our response only if it is explicitly asked for*/
                    systemTextJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.codemaze.apiroot+json");/*For the GetRoot*/
                }
                var xmlOutputFormatter = config.OutputFormatters
                .OfType<XmlDataContractSerializerOutputFormatter>()?
                .FirstOrDefault();
                if (xmlOutputFormatter != null)
                {
                    xmlOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.codemaze.hateoas+xml");/*What we want to do is to enable links in our response only if it is explicitly asked for*/
                    xmlOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.codemaze.apiroot+xml");/*For the GetRoot*/
                }
            });
        }

        /*adding service API
versioning to the service collection. We are also using a couple of
properties to initially configure versioning*/
        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                /*adds the API version to the response header*/
                opt.ReportApiVersions = true;
                /*It specifies the default API version if the client doesn’t send one*/
                opt.AssumeDefaultVersionWhenUnspecified = true;
                /*sets the default version count*/
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                /*enable  HTTP Header Versioning*/
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }


        /*Adding Cache-Store Service  */
        public static void ConfigureResponseCaching(this IServiceCollection services) =>
        services.AddResponseCaching();

        /*Add http cache header 
         Example of http cache header : Cache-Control,
E       xpires, Etag, and Last-Modified
         */
        public static void ConfigureHttpCacheHeaders(this IServiceCollection services) =>
services.AddHttpCacheHeaders(
    (expirationOpt) =>
    {
        expirationOpt.MaxAge = 65;
        expirationOpt.CacheLocation = CacheLocation.Private;/*this is a private
                    cache with an age of 65 seconds. Because it is a private cache, our API
                    won’t cache it.*/
    },
        (validationOpt) =>
        {
            validationOpt.MustRevalidate = true;
        });


        /*extension to configure asp.net identity*/
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 10;
                o.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();
        }

        /*JWT configuration */
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration
configuration)
        {
            var jwtConfiguration = new JwtConfiguration();
            configuration.Bind(jwtConfiguration.Section, jwtConfiguration);
            var secretKey = Environment.GetEnvironmentVariable("SECRET");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfiguration.ValidIssuer,
                    ValidAudience = jwtConfiguration.ValidAudience,
                    IssuerSigningKey = new
                     SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

        /*register and configure the
JwtConfiguration class in the ServiceExtensions class*/
        public static void AddJwtConfiguration(this IServiceCollection services,
IConfiguration configuration) =>
services.Configure<JwtConfiguration>(configuration.GetSection("JwtSettings"));


        /*Configure Swagger */
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Code Maze API",
                    Version = "v1",
                    Description = "CompanyEmployees API by CodeMaze",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "John Doe",
                        Email = "John.Doe@gmail.com",
                        Url = new Uri("https://twitter.com/johndoe"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "CompanyEmployees API LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                s.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Code Maze API",
                    Version = "v2"
                });

                var xmlFile = $"{typeof(Presentation.AssemblyReference).Assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);

                /*Adding Authorization Support*/
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });


            });

        }
    }


}
