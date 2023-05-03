using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Web;

const string SIGNALR_CORS_POLICY = "SIGNALR";
var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddSignalR()
    .AddStackExchangeRedis(builder.Configuration["Redis:ConnectionString"])
    ;
services.AddControllers();
services.AddRazorPages()
    .AddRazorRuntimeCompilation();

services.AddCors(options =>
{
    options.AddPolicy(SIGNALR_CORS_POLICY,
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            //.AllowCredentials()
            );
});

var app = builder.Build();
app.UseDeveloperExceptionPage();

app.UseCors(SIGNALR_CORS_POLICY);
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<TestHub>("/test");
    endpoints.MapRazorPages();
    endpoints.MapControllers();
});

app.Run();