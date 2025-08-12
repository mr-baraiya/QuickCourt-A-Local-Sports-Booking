using Microsoft.EntityFrameworkCore;
using QuickCourtBackend.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});


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


app.UseCors("AllowReactApp");
app.UseHttpsRedirection();

// Add Authentication and Authorization middleware if applicable
// app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
