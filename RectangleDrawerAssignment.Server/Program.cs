using RectangleDrawerAssignment.Server.Services;
using RectangleDrawerAssignment.Server.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient< IRectangleSerializer, JsonRectangleSerializer>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("http://localhost:4200", "https://localhost:4200") 
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
