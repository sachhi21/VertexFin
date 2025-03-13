using System;
using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.Persistence; // Ensure this is correct

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add services to the container.
var ConnectionString = builder.Configuration.GetConnectionString("portrecDBConnection");
builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlServer(ConnectionString, d => d.MigrationsAssembly("UserService")));
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();