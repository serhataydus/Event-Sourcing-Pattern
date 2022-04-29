using EventSourcing.WebApi.BackgroundServices;
using EventSourcing.WebApi.Data;
using EventSourcing.WebApi.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEventStore(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<IProductDbContext, ProductDbContext>(options =>
         options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")),
         ServiceLifetime.Transient);
builder.Services.AddHostedService<ProductReadModelEventStore>();

WebApplication? app = builder.Build();

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
