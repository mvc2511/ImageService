using ImageService.Data;
using ImageService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Grpc.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios gRPC y DbContext
builder.Services.AddGrpc();
builder.Services.AddDbContext<ImageContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Mapear servicios gRPC
app.MapGrpcService<ImageServiceImpl>();

// Ruta para manejar solicitudes no gRPC
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

// Ejecutar la aplicación
app.Run();
