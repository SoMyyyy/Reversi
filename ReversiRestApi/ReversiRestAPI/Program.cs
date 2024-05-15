using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.OpenApi.Models;
using ReversieISpelImplementatie.Model;
using ReversiRestAPI.DAL;
using Newtonsoft.Json;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddScoped<SpelAccessLayer>();
builder.Services.AddDbContext<ReversiAPI_DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReversiAPIDB")));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ReversiRestApi", Version = "v1" });
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    // options.AddPolicy("FrontEnd",
    //     builder => builder.WithOrigins("http://localhost:63342", "https://localhost:63342").WithMethods("PUT").AllowAnyHeader());
    options.AddPolicy("AllPorts",
        builder => builder.WithOrigins("http://localhost:63342", "https://localhost:63342","http://localhost:7103", "https://localhost:7103", "https://localhost:44349").WithMethods("PUT").AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReversiRestApi v1"));
}

app.UseHttpsRedirection();

//! added the cors to use the frontend while checking the bord game and place of fische
// app.UseCors("FrontEnd");
app.UseCors("AllPorts");

app.UseAuthorization();
app.MapControllers();
app.Run();