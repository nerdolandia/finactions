using FinActions.Api.Host.Extensions;
using FinActions.Api.Host.OpenApi;
using FinActions.Application;
using FinActions.Application.Exceptions;
using Serilog;

Console.WriteLine("Starting FinActions.Api.Host");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplication(builder.Configuration)
    .AddAuthentication(builder.Configuration)
    .AddAutoMapper(typeof(FinActionsMappingProfile))
    .AddSerilogFromAppSettings(builder.Configuration)
    .AddOpenApi(options =>
    {
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    })
    .AddProblemDetails()
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/openapi/v1.json", "FinActions.API");
});

//app.UseHttpsRedirection();

app.UseCors(x => { x.AllowAnyHeader(); x.AllowAnyOrigin(); x.AllowAnyMethod(); });

app.UseExceptionHandler();

app.UseStatusCodePages();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
