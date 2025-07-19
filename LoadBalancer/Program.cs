var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddReverseProxy()
    .LoadFromMemory(
        [
            new Yarp.ReverseProxy.Configuration.RouteConfig
            {
                RouteId = "default",
                ClusterId = "backend-cluster",
                Match = new()
                {
                    Path = "{**catch-all}"
                }
            }
        ],
        [
            new Yarp.ReverseProxy.Configuration.ClusterConfig
            {
                ClusterId = "backend-cluster",
                Destinations = new Dictionary<string, Yarp.ReverseProxy.Configuration.DestinationConfig>
                {
                    ["server1"] = new() { Address = "http://localhost:5001" },
                    ["server2"] = new() { Address = "http://localhost:5002" },
                },
                LoadBalancingPolicy = "RoundRobin"
            }
        ]
    );

var app = builder.Build();

app.MapReverseProxy();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run("http://localhost:5000");