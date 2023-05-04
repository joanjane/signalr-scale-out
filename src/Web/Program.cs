using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Web;

const string REDIS_BACKPLANE = "redis";
const string SQLSERVER_BACKPLANE = "sqlserver";

const string SIGNALR_CORS_POLICY = "SIGNALR";
var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var backplane = builder.Configuration.GetValue("SignalR:Backplane", REDIS_BACKPLANE);
var signalRBuilder = services.AddSignalR();
if (backplane == REDIS_BACKPLANE)
{
    signalRBuilder.AddStackExchangeRedis(builder.Configuration["Redis:ConnectionString"]);
} 
else if (backplane == SQLSERVER_BACKPLANE)
{
    signalRBuilder.AddSqlServer(o =>
    {
        o.ConnectionString = builder.Configuration["Sql:ConnectionString"];
        // See above - attempts to enable Service Broker on the database at startup
        // if not already enabled. Default false, as this can hang if the database has other sessions.
        o.AutoEnableServiceBroker = true;
        // Every hub has its own message table(s). 
        // This determines the part of the table named that is derived from the hub name.
        // IF THIS IS NOT UNIQUE AMONG ALL HUBS, YOUR HUBS WILL COLLIDE AND MESSAGES MIX.
        o.TableSlugGenerator = hubType => hubType.Name;
        // The number of tables per Hub to use. Adding a few extra could increase throughput
        // by reducing table contention, but all servers must agree on the number of tables used.
        // If you find that you need to increase this, it is probably a hint that you need to switch to Redis.
        o.TableCount = 1;
        // The SQL Server schema to use for the backing tables for this backplane.
        o.SchemaName = "SignalRCore";
    });
} 
else
{
    backplane = string.Empty;
}

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
            );
});

var app = builder.Build();
var logger = app.Services.GetService<ILogger<Program>>();
logger.LogInformation($"Using SignalR backplane '{backplane}'");

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