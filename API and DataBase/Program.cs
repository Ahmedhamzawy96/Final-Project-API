using API_and_DataBase.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
string ploicy = " ";

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CompanyContext>(Options => Options.UseSqlServer("server=(localdb)\\MSSQLLocalDB;database=ElMokhtarDB;Trusted_Connection=True"));
//builder.Services.AddDbContext<CompanyContext>(Options => Options.UseSqlServer("Data Source=SQL8004.site4now.net;Initial Catalog=db_a8b9a3_elmokhtar;User Id=db_a8b9a3_elmokhtar_admin;Password=@A123456"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my_sercret_key_123456"))

    };
});

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
