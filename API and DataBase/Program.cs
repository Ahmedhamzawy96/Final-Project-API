using API_and_DataBase.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
string ploicy = " ";

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<CompanyContext>(Options => Options.UseSqlServer("server=.;database=ElMokhtarDB;user id=Hamzawy;password=Hamzawy9678"));
builder.Services.AddDbContext<CompanyContext>(Options => Options.UseSqlServer("server=.;database=ElMokhtarDB;Trusted_Connection=True"));

builder.Services.AddCors(options => options.AddPolicy(ploicy, builder =>
{
    builder.AllowAnyOrigin();
    builder.AllowAnyMethod();
    builder.AllowAnyHeader();
}));
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors(ploicy);
app.UseAuthorization();
app.MapControllers();

app.Run();
