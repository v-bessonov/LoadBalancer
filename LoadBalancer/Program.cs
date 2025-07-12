var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



// Add YARP services
builder.Services.AddReverseProxy()
    .LoadFromMemory(
        new[]
        {
            new Yarp.ReverseProxy.Configuration.RouteConfig
            {
                RouteId = "default",
                ClusterId = "backend-cluster",
                Match = new()
                {
                    Path = "{**catch-all}"
                }
            }
        },
        new[]
        {
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
        }
    );

var app = builder.Build();

// Use the reverse proxy
app.MapReverseProxy();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run("http://localhost:5000");