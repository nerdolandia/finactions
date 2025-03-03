using FinActions.Api.Host.Extensions;
using FinActions.Api.Host.OpenApi;
using FinActions.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddAuthentication(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/openapi/v1.json", "FinActions.API");
});

//app.UseHttpsRedirection();

app.UseCors(x => { x.AllowAnyHeader(); x.AllowAnyOrigin(); x.AllowAnyMethod(); });


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
