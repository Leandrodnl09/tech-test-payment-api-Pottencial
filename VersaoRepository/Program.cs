using Microsoft.EntityFrameworkCore;
using VersaoRepository.Context;
using VersaoRepository.Interface;
using VersaoRepository.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IPedidos, PedidosRepository>();
builder.Services.AddTransient<IVendedor, VendedoresRepository>();

builder.Services.AddDbContext<BdVendasContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoVendas")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
