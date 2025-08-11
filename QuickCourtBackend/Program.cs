using Microsoft.EntityFrameworkCore;
using QuickCourtBackend.Models;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext early
builder.Services.AddDbContext<QuickCourtContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("myConnectionString"))
);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add Authentication and Authorization middleware if applicable
// app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
