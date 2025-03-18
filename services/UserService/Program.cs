using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using PortRec.RepositoryLayer.Repository;
using SharedRepository.IRepository;
using UserService.src.Application.IUserService;
using UserService.src.Application.UserService;
using VertexFin.Domain.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// Add services to the container.
builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddScoped<IUserService, UserServices>();
var ConnectionString = builder.Configuration.GetConnectionString("portrecDBConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(ConnectionString, d => d.MigrationsAssembly("UserService")));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapControllers();
app.Run();