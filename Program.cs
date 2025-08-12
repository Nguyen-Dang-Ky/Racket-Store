using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serberus_Racket_Store.Data;
using AutoMapper;
using Serberus_Racket_Store.Profiles;
using Serberus_Racket_Store.MappingProfiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowPolicy", policy =>
    {
        policy
            .AllowAnyOrigin() // Cho phép tất cả origin (có thể thay bằng WithOrigins("http://localhost:5500") để cụ thể)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
// Add services to the container.
builder.Services.AddDbContext<SeberusDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthentication();

//Đăng ký AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}
//app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting(); 

app.UseCors("MyAllowPolicy");

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
