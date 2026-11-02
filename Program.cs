using ADONET_Practico.Data;
using ADONET_Practico.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("Productos")
    ?? throw new InvalidOperationException("No se configuró ConnectionStrings:Productos.");
builder.Services.AddSingleton<IProductoRepository>(_ => new ProductoRepository(connectionString));

var app = builder.Build();
DatabaseInitializer.Initialize(connectionString);

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();
