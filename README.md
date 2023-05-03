# SignalR scale-out example

Example for scaling out signalr (asp.net 6) on multiple front-end servers using Redis as backplane. 
This is a fork of [bremnes](https://github.com/bremnes/signalr-scale-out) repository, which simplifies setup and updates all dependencies.

![](https://docs.microsoft.com/en-us/aspnet/signalr/overview/performance/scaleout-in-signalr/_static/image2.png)

[source](https://docs.microsoft.com/en-us/aspnet/signalr/overview/performance/scaleout-in-signalr)

## Usage:

### Docker
1. Open a command line into src folder.
2. Build docker images `docker compose build web`
3. Launch Redis and 2 web instances: `docker compose up`
4. Test Web 1 on http://localhost:3010
5. Test Web 2 on http://localhost:3011

### .NET CLI
1. Open a command line into src folder.
2. Launch redis with docker: `docker compose up redis`
3. Launch one instance of Web with `dotnet run --launch-profile Web`
4. Launch a second instance of Web with `dotnet run --launch-profile Web2`
5. Test Web 1 on http://localhost:3010
6. Test Web 2 on http://localhost:3011

## Resources:
- https://learn.microsoft.com/en-us/aspnet/core/signalr/hubcontext?view=aspnetcore-7.0
- https://learn.microsoft.com/en-us/aspnet/core/signalr/hubs?view=aspnetcore-7.0
- https://learn.microsoft.com/es-es/aspnet/core/tutorials/signalr?view=aspnetcore-7.0&tabs=visual-studio
