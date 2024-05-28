

using CompanyEmployees.Extensions;
using CompanyEmployees.Presentation.ActionFilters;
using Contracts;
using MediatR;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using NLog;
using FluentValidation;
using Application.Behaviors;
using CompanyEmployees.Reflection;
using Entities.ErrorModel;
using Shared.DataTransferObjects.Employee;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// configuration the nlog
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "nlog.config"));
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));/*add the service IMapper mapper in IOC */
/*IPipelineBehavior using for the Influent validation class*/
builder.Services.AddTransient(typeof(IPipelineBehavior<,>),
typeof(ValidationBehavior<,>));

builder.Services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly);

builder.Services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();

/*suppressing a default model state validation that is
implemented due to the existence of the [ApiController] attribute in
all API controllers*/
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


/*register action filter to validate media type */
builder.Services.AddScoped<ValidateMediaTypeAttribute>();

/*=> register response caching in the IOC container*/
builder.Services.ConfigureResponseCaching();
/*=>register http cache header in the IOC container*/
builder.Services.ConfigureHttpCacheHeaders();
/*Without this code, our API wouldn’t work, and wouldn’t know where to
route incoming requests. But now, our app will find all of the controllers
inside of the Presentation project and configure them with the
framework. They are going to be treated the same as if they were defined
conventionally*/
builder.Services.AddControllers(config =>
{

    config.ReturnHttpNotAcceptable = true; /*tells
                                the server that if the client tries to negotiate for the media type the
                                server doesn’t support, it should return the 406 Not Acceptable status code*/

    config.RespectBrowserAcceptHeader = true;

    config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
    /*adding cache prfile */
    config.CacheProfiles.Add("120SecondsDuration", new CacheProfile
    {
        Duration = 120
    });
})
    /*options to enable the server to format the XML
response when the client tries negotiating for it*/
    .AddXmlDataContractSerializerFormatters()
/*Active the csv custom formatter*/
.AddCustomCSVFormatter()
/*to the assembly namespace where the controllers are located, in our exalple are located in CompanyEmployees.Presentation project*/
.AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

/*Custom Media type */
builder.Services.AddCustomMediaTypes();

/*plug the configure versioning service*/
builder.Services.ConfigureVersioning();

/*configure asp.net identity*/
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
/*Add the Bearer Check in the pipeline*/
builder.Services.ConfigureJWT(builder.Configuration);
/*we will use IOption*/
builder.Services.AddJwtConfiguration(builder.Configuration);

/*inject the swagger configuration in IOC */
builder.Services.ConfigureSwagger();

/*The AddMediatR method will scan the project assembly that contains the
handlers that we are going to use to handle our business logic. Since we
are going to place those handlers in the Application project, we are
using the Application’s assembly as a parameter.*/
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Application.AssemblyReference).Assembly));


/*plug  service mail Sender*/
builder.Services.ConfigureInfrastructureServices(builder.Configuration);

var app = builder.Build();

/*extract the ILoggerManager service inside the logger
variable and plug the extension Middleware (to manage global Exception ) 
It is important to know that we have to
extract the ILoggerManager service after the var app =
builder.Build() code line because the Build method builds the
WebApplication and registers all the services added with IOC
 */
var logger = app.Services.GetRequiredService<ILoggerManager<ErrorDetails>>();
app.ConfigureExceptionHandler(logger);


// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
    app.UseHsts();/*app.UseHsts() will add middleware for using HSTS, which adds the
Strict-Transport-Security header*/



app.UseHttpsRedirection();


/*app.UseStaticFiles() enables using static files for the request. If
we don’t set a path to the static files directory, it will use a wwwroot
folder in our project by default.*/
app.UseStaticFiles();

/*app.UseForwardedHeaders() will forward proxy headers to the
current request. This will help us during application deployment. Pay
attention that we require Microsoft.AspNetCore.HttpOverrides
using directive to introduce the ForwardedHeaders enumeratio*/
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

/*Microsoft recommends having UseCors before
UseResponseCaching*/
app.UseCors("CorsPolicy");
/*add caching to the application middleware*/
app.UseResponseCaching();
app.UseHttpCacheHeaders();
/*plug the middelware when we use asp.net identity*/
app.UseAuthentication();
app.UseAuthorization();

/*plug swagger*/
app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Code Maze API v1");
    s.SwaggerEndpoint("/swagger/v2/swagger.json", "Code Maze API v2");
});

app.MapControllers();

app.Run();

/*By using AddNewtonsoftJson, we are replacing the System.Text.Json
formatters for all JSON content. We don’t want to do that so, we are
going ton add a simple workaround in the Program class*/
NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
.Services.BuildServiceProvider()
.GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
.OfType<NewtonsoftJsonPatchInputFormatter>().First();